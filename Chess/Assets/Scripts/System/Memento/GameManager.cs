using Plugins.MissionCore.Core;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public ChessSide PlayerSide {  get; private set; }
    public int LastLevelNumber {  get; set; }
    public PlayerData PlayerData { get; private set; }
    public int MainChessSideIndex { get; private set; }

    [Header("Game data")]
    public GameGeneralSettings generalSettings;

    public Action OnMatchStart;
    public Action<bool> OnMatchEnd;
    public Action OnEnterNextLevel;

    public LevelHandler LevelHandler;
    public Image LoadingImage;
    public int LoadingDelaySeconds;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnMatchStart += StartMatch;
        OnMatchEnd += EndMatch;
        OnEnterNextLevel += EnterNextLevel;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            LevelHandler = FindFirstObjectByType<LevelHandler>();
            if (!LevelHandler.Equals(null)) LoadGame();
        }
    }
    private IEnumerator LoadingCooldown()
    {
        yield return new WaitForSeconds(LoadingDelaySeconds);

        LoadingImage.gameObject.SetActive(false);
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
        var player = LevelHandler.PlayerMovementController.GetComponent<Player>();
        
        PlayerData.SetStats(LastLevelNumber, player.gameObject.transform);

        Debug.LogError(player.Level);

        SaveGame(PlayerData);

        LevelHandler.PlayerMovementController.gameObject.SetActive(false);
        LevelHandler.ChessBoardHandler.OnGameStateChange?.Invoke(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void EndMatch(bool isHaveWon)
    {
        LevelHandler.OnEndMatch?.Invoke();
        LevelHandler.ChessBoardHandler.OnGameStateChange?.Invoke(false);

        if (isHaveWon) LevelHandler.WinsCounts++;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        LevelHandler.PlayerMovementController.gameObject.SetActive(true);

        SaveGame(PlayerData);
    }

    private void EnterNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
         
        SceneManager.LoadScene(nextSceneIndex);

        if (nextSceneIndex > LastLevelNumber) LastLevelNumber = nextSceneIndex;

        PlayerData.SetStats(LastLevelNumber);
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
        player?.SetStats(LastLevelNumber, player.transform);

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
    public float[] position = new float[3];

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
        SetLevelStats(1);

        position[0] = 0f;
        position[1] = 0f;
        position[2] = 0f;
    }
    public PlayerData PlayerToPlayerData(Player player)
    {
        return new PlayerData(player.FullHealth, player.Strength, player.Defence, player.transform);
    }
    public void SetStats(int level)
    {
        SetLevelStats(level);

        position[0] = 0f;
        position[1] = 0f;
        position[2] = 0f;
    }
    public void SetStats(int level, Transform pTransform)
    {
        SetLevelStats(level);

        position[0] = pTransform.position.x;
        position[1] = pTransform.position.y;
        position[2] = pTransform.position.z;
    }

    private void SetLevelStats(int level)
    {
        if (level == 1)
        {
            fullHealth = 10f;
            strength = 5;
            defence = 5;
        }
        if (level == 2)
        {
            fullHealth = 25f;
            strength = 10;
            defence = 10;
        }
        if (level == 3)
        {
            fullHealth = 75f;
            strength = 20;
            defence = 15;
        }
        if (level == 4)
        {
            fullHealth = 100f;
            strength = 25;
            defence = 15;
        }
    }
}
