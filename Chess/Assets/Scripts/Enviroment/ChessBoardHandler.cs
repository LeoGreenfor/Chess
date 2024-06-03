using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardHandler : MonoBehaviour
{
    [SerializeField] private ChessBoardCell[] cells;
    [SerializeField] private ChessPiece[] pieces;

    public Action<bool> OnGameStateChange;
    public Action OnPlayerMove;

    public bool IsGameBegin {  get; private set; }

    private void Start()
    {
        OnGameStateChange += SetBeginGame;
    }

    private void SetBeginGame(bool state)
    {
        IsGameBegin = state;
    }

    private void FixedUpdate()
    {
        if (!IsGameBegin) return;
        

    }
}
