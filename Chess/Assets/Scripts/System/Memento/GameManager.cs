using Plugins.MissionCore.Core;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public ChessSide PlayerSide {  get; private set; }
    public int LastLevelNumber {  get; set; }
    public PlayerData PlayerData { get; private set; }
    public int MainChessSideIndex { get; private set; }

    [Header("Game data")]
    public GameGeneralSettings generalSettings;

    public Action OnMatchStart;
    public Action OnMatchEnd;

    private ChessBoardHandler _chessBoardHandler;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnMatchStart += StartMatch;
        OnMatchEnd += EndMatch;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _chessBoardHandler = FindFirstObjectByType<ChessBoardHandler>();
        if (FindFirstObjectByType<Player>()) LoadGame();
        Debug.LogError(PlayerSide.ToString() + " " + PlayerData.ToString() + " " + LastLevelNumber.ToString());
    }

    public void SetPlayerSide(string playerSide)
    {
        if (playerSide.ToLower() == ChessSide.White.ToString().ToLower())
        {
            PlayerSide = ChessSide.White;
            MainChessSideIndex = 0;
            return;
        }
        if (playerSide.ToLower() == ChessSide.Black.ToString().ToLower())
        {
            PlayerSide = ChessSide.Black;
            MainChessSideIndex = 1;
            return;
        }

        Debug.LogError("wrong chess side");
    }

    private void StartMatch()
    {
        var player = FindFirstObjectByType<PlayerMovementController>().GetComponent<Player>();
        
        PlayerData.SetStats(player.FullHealth, player.Strength, player.Defence, player.gameObject.transform);

        SaveGame(PlayerData);

        player.GetComponent<PlayerMovementController>().MainCamera.enabled = false;
        _chessBoardHandler.OnGameStateChange?.Invoke(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void EndMatch()
    {
        _chessBoardHandler.OnGameStateChange?.Invoke(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        var player = FindFirstObjectByType<PlayerMovementController>().GetComponent<Player>();
        player.GetComponent<PlayerMovementController>().MainCamera.enabled = true;

        SaveGame(PlayerData);
    }

    #region Memento 
    // Saves the current state inside a memento.
    public void SaveGame(PlayerData player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, generalSettings.SaveGameFilePath);
        Debug.LogError(path);
        FileStream fileStream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(fileStream, Save(player));
        fileStream.Close();
    }
    public Memento Save(PlayerData player)
    {
        return new Memento(PlayerSide, player, LastLevelNumber);
    }
    public Memento Save()
    {
        Debug.LogError(PlayerSide.Equals(null) + " " + PlayerData.Equals(null) + " " + LastLevelNumber.Equals(null));
        return new Memento(PlayerSide, PlayerData, LastLevelNumber);
    }

    // Restores the Originator's state from a memento object.
    public void LoadGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Path.Combine(Application.persistentDataPath, generalSettings.SaveGameFilePath);

        if (File.Exists(filePath))
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                var restoredData = formatter.Deserialize(stream) as IMemento;
                Restore(restoredData);
            }
        }
        else
        {
            var player = new PlayerData();
            SaveGame(player);
        }
    }
    public void Restore(IMemento memento)
    {
        if (memento is not Memento)
        {
            Debug.LogError($"Unknown memento implementation found: {memento.GetType().FullName}");
            return;
        }

        PlayerSide = memento.GetPlayerChessSide();

        var player = FindFirstObjectByType<PlayerMovementController>()?.GetComponent<Player>();
        PlayerData = memento.GetPlayer();
        player?.SetStats(PlayerData.fullHealth, PlayerData.strength, PlayerData.defence, player.transform);

        LastLevelNumber = memento.GetLastLevelNumber();
    }

    [Serializable]
    public class Memento : IMemento
    {
        private readonly ChessSide _playerSide;
        private readonly PlayerData _player;
        private readonly int _lastLevelNumber;

        public Memento(ChessSide chessSide, PlayerData player, int lastLevel)
        {
            _playerSide = chessSide;
            _player = player;
            _lastLevelNumber = lastLevel;
        }

        public int GetLastLevelNumber()
        {
            return _lastLevelNumber;
        }

        public PlayerData GetPlayer()
        {
            return _player;
        }

        public ChessSide GetPlayerChessSide()
        {
            return _playerSide;
        }
    }
    #endregion
}
public enum ChessSide
{
    White,
    Black
}
[Serializable]
public class PlayerData
{
    // Stats
    public float fullHealth;
    public float strength;
    public float defence;
    // Transform
    //public Transform transform;
    //public Vector3 position;
    public float[] position = new float[3];
    //public Quaternion rotation;

    public PlayerData(float pFullHealth, float pStrength, float pDefence, Transform pTransform)
    {
        fullHealth = pFullHealth;
        strength = pStrength;
        defence = pDefence;

        position[0] = pTransform.position.x;
        position[1] = pTransform.position.y;
        position[2] = pTransform.position.z;
    }

    /// <summary>
    /// Creates Player with Lv1 - pawn
    /// </summary>
    public PlayerData()
    {
        fullHealth = 10f;
        strength = 5f;
        defence = 5f;

        position[0] = 0f;
        position[1] = 0f;
        position[2] = 0f;
    }
    public PlayerData PlayerToPlayerData(Player player)
    {
        return new PlayerData(player.FullHealth, player.Strength, player.Defence, player.transform);
    }
    public void SetStats(float pFullHealth, float pStrength, float pDefence, Transform pTransform)
    {
        fullHealth = pFullHealth;
        strength = pStrength;
        defence = pDefence;

        position[0] = pTransform.position.x;
        position[1] = pTransform.position.y;
        position[2] = pTransform.position.z;
    }
}
