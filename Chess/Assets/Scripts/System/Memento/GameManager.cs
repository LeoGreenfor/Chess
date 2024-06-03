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

    [Header("Game data")]
    public GameGeneralSettings generalSettings;

    [Header("Game control")]
    [SerializeField] private Player chessPlayer;
    [SerializeField] private Player freeMovePlayer;

    public Action OnMatchStart;
    public Action OnMatchEnd;


    private int _mainChessSideIndex;
    private ChessBoardHandler _chessBoardHandler;

    private void Start()
    {
        OnMatchStart += StartMatch;
        OnMatchEnd += EndMatch;
        _chessBoardHandler = FindFirstObjectByType<ChessBoardHandler>();
    }

    public void SetPlayerSide(string playerSide)
    {
        if (playerSide.ToLower() == ChessSide.White.ToString().ToLower())
        {
            PlayerSide = ChessSide.White;
            _mainChessSideIndex = 0;
            return;
        }
        if (playerSide.ToLower() == ChessSide.Black.ToString().ToLower())
        {
            PlayerSide = ChessSide.Black;
            _mainChessSideIndex = 1;
            return;
        }

        Debug.LogError("wrong chess side");
    }
    public Player GetFreeMovePlayer() => freeMovePlayer;
    public Player GetChessPlayer() => chessPlayer;

    private void StartMatch()
    {
        //SaveGame();
        freeMovePlayer.gameObject.SetActive(false);
        chessPlayer.gameObject.SetActive(true);
        _chessBoardHandler.OnGameStateChange?.Invoke(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void EndMatch()
    {
        freeMovePlayer.gameObject.SetActive(true);
        chessPlayer.gameObject.SetActive(false);
        _chessBoardHandler.OnGameStateChange?.Invoke(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #region Memento 
    // Saves the current state inside a memento.
    public void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, generalSettings.SaveGameFilePath);
        Debug.LogError(path);
        FileStream fileStream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(fileStream, Save());
        fileStream.Close();
    }
    public IMemento Save()
    {
        return new Memento(PlayerSide, freeMovePlayer, LastLevelNumber);
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
            SaveGame();
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
        freeMovePlayer = memento.GetPlayer();
        LastLevelNumber = memento.GetLastLevelNumber();
    }

    private class Memento : IMemento
    {
        private readonly ChessSide _playerSide;
        private readonly Player _player;
        private readonly int _lastLevelNumber;

        public Memento(ChessSide chessSide, Player player, int lastLevel)
        {
            _playerSide = chessSide;
            _player = player;
            _lastLevelNumber = lastLevel;
        }

        public int GetLastLevelNumber()
        {
            return _lastLevelNumber;
        }

        public Player GetPlayer()
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
