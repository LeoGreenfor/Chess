using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardHandler : MonoBehaviour
{
    [SerializeField] private ChessBoardCell[] cells;
    [SerializeField] private ChessPiece[] pieces;

    private bool isGameBegin;

    private void OnCollisionEnter(Collision collision)
    {
        isGameBegin = true;
    }

    private void FixedUpdate()
    {
        if (!isGameBegin) return;
        

    }
}
