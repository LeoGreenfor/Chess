using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardHandler : MonoBehaviour
{
    [SerializeField] private ChessBoardCell[] cells;
    [SerializeField] private ChessPieceController[] pieces;
    [SerializeField] private PlayerController chessPlayer;

    public Action<bool> OnGameStateChange;
    public Action OnPlayerMakeMove;

    public bool IsGameBegin {  get; private set; }

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
    }

    private void FixedUpdate()
    {
        if (!IsGameBegin) return;
        

    }

    private void MovePlayer(Transform newPlayerPosition)
    {
        chessPlayer.MoveTo(newPlayerPosition);
    }

    private void MakeMove()
    {

    }
}
