using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardHandler : MonoBehaviour
{
    [SerializeField] private ChessBoardCell[] cells;
    [SerializeField] private ChessPieceController[] pieces;
    [SerializeField] private PlayerController chessPlayer;
    [SerializeField] private Camera boardCamera;

    public Action<bool> OnGameStateChange;
    public Action OnPlayerMakeMove;

    public bool IsGameBegin {  get; private set; }
    public int WinsCounts;

    private void Start()
    {
        OnGameStateChange += SetBeginGame;

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].OnPlayerMove += MovePlayer;
        }
    }

    private void SetBeginGame(bool state)
    {
        IsGameBegin = state;
        boardCamera.gameObject.SetActive(IsGameBegin);

        if (IsGameBegin)
        {
            SetChessPieces();
        }
    }

    private void SetChessPieces()
    {
        int piecesCount = WinsCounts++;


    }

    private void MovePlayer(Transform newPlayerPosition)
    {
        chessPlayer.MoveTo(newPlayerPosition);
    }

    private void MakeMove()
    {

    }
}
