using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameGeneralSettings;

public class ChessBoardHandler : MonoBehaviour
{
    [SerializeField] private BoardCells[] rows;

    [SerializeField] private ChessPieceController[] piecesPrefabs;
    [SerializeField] private ChessPieceController[] piecesOnBoard;

    [SerializeField] private PlayerController chessPlayerControllerPrefab;
    [SerializeField] private PlayerController chessPlayer;

    [SerializeField] private Camera boardCamera;

    public Action<bool> OnGameStateChange;
    public Action OnPlayerMakeMove;

    public bool IsGameBegin {  get; private set; }
    public int WinsCounts;

    private int _levelNumber;
    private int _boardSideToInt;

    private void Start()
    {
        OnGameStateChange += SetBeginGame;
        OnPlayerMakeMove += MakeMove;

        _boardSideToInt = GameManager.Instance.MainChessSideIndex;

        ChessPiece[] chessPieces = GameManager.Instance.generalSettings.chessPiecesPrefabs[_boardSideToInt].chessPieces;
        piecesPrefabs = new ChessPieceController[chessPieces.Length];

        for (int i = 0; i < chessPieces.Length; i++)
        {
            Debug.LogError($"chessPieces.Length: {chessPieces.Length}, pieces.Length: {piecesPrefabs.Length}, i: {i}, " +
                $"have controller: {chessPieces[i].GetComponent<ChessPieceController>()}");
            piecesPrefabs[i] = chessPieces[i].GetComponent<ChessPieceController>();
        }
        
        _levelNumber = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < rows.Length; i++)
            for (int j = 0; j < rows[i].cells.Length; j++)
                rows[i].cells[j].OnPlayerMove += MovePlayer;
    }

    private void SetBeginGame(bool state)
    {
        IsGameBegin = state;
        boardCamera.gameObject.SetActive(IsGameBegin);

        if (IsGameBegin)
        {
            SetChessPieces();

            var entity = chessPlayerControllerPrefab.SpawnEntity(rows[0].cells[1]);
            chessPlayer = entity.GetComponent<PlayerController>();
            
            var player = chessPlayer.gameObject.GetComponent<Player>();
            player.UpdateLevel(_levelNumber);
            chessPlayer.CreateEntity(player);
            chessPlayer.SetCurrentCell(rows[0].cells[1]);
        }
    }

    private void SetChessPieces()
    {
        int piecesCount = WinsCounts + 1;
        int index = rows.Length - 1;
        piecesOnBoard = new ChessPieceController[piecesCount];

        for (int i = 0; i < piecesCount; i++)
        {
            if (i % 2 == 0) 
            {
                var entity = piecesPrefabs[_levelNumber].SpawnEntity(rows[index].cells[i]);
                piecesOnBoard[i] = entity as ChessPieceController;
                piecesOnBoard[i].SetCurrentCell(rows[index].cells[i]);
            }
            else
            {
                var entity = piecesPrefabs[_levelNumber + 1].SpawnEntity(rows[index].cells[i]);
                piecesOnBoard[i] = entity as ChessPieceController;
                piecesOnBoard[i].SetCurrentCell(rows[index].cells[i]);
            }
            
        }
    }

    private void MovePlayer(ChessBoardCell newPlayerPosition)
    {
        chessPlayer.SetOnTurn(true);
        chessPlayer.MoveTo(newPlayerPosition);
        OnPlayerMakeMove?.Invoke();
    }

    private void MakeMove()
    {

    }

    [System.Serializable]
    private class BoardCells
    {
        public ChessBoardCell[] cells;
    }
}
