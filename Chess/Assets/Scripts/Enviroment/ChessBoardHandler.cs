using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameGeneralSettings;

public class ChessBoardHandler : MonoBehaviour
{
    [SerializeField] private BoardCells[] rows;
    [SerializeField] private ChessPieceController[] pieces;
    [SerializeField] private PlayerController chessPlayerController;
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

        _boardSideToInt = GameManager.Instance.MainChessSideIndex;

        ChessPiece[] chessPieces = GameManager.Instance.generalSettings.chessPiecesPrefabs[_boardSideToInt].chessPieces;
        pieces = new ChessPieceController[chessPieces.Length];

        for (int i = 0; i < chessPieces.Length; i++)
        {
            Debug.LogError($"chessPieces.Length: {chessPieces.Length}, pieces.Length: {pieces.Length}, i: {i}, " +
                $"have controller: {chessPieces[i].GetComponent<ChessPieceController>()}");
            pieces[i] = chessPieces[i].GetComponent<ChessPieceController>();
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

            chessPlayerController.SpawnEntity(rows[0].cells[1]);
        }
    }

    private void SetChessPieces()
    {
        int piecesCount = WinsCounts + 1;
        int index = rows.Length - 1;

        for (int i = 0; i < piecesCount; i++)
        {
            if (i % 2 == 0) pieces[_levelNumber].SpawnEntity(rows[index].cells[i]);
            else pieces[_levelNumber + 1].SpawnEntity(rows[index].cells[i]);
            
        }
    }

    private void MovePlayer(ChessBoardCell newPlayerPosition)
    {
        chessPlayerController.MoveTo(newPlayerPosition);
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
