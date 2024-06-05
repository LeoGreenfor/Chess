using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameGeneralSettings;

public class ChessBoardHandler : MonoBehaviour
{
    [SerializeField] private ChessBoardCell[] cells;
    [SerializeField] private ChessPiece[] pieces;
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

        _boardSideToInt = GameManager.Instance.MainChessSideIndex;
        pieces = GameManager.Instance.generalSettings.chessPiecesPrefabs[_boardSideToInt].chessPieces;
        _levelNumber = SceneManager.GetActiveScene().buildIndex;

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
        int piecesCount = WinsCounts + 1;
        var chesspieceRotation = new Quaternion(0, 180, 0, 0);

        Debug.LogError(piecesCount + " " + cells.Length);

        for (int i = 0; i < piecesCount; i++)
        {
            int index = cells.Length - 1 - i;

            if (index >= 0) // Перевірка, чи індекс не виходить за межі масиву
            {
                Debug.LogError(cells[index]);
                if (index % 2 == 0) Instantiate(pieces[_levelNumber].gameObject, cells[index].transform.position, chesspieceRotation);
                else Instantiate(pieces[_levelNumber + 1].gameObject, cells[index].transform.position, chesspieceRotation);
            }
            else
            {
                Debug.LogError("Індекс виходить за межі масиву");
                break;
            }
        }

        /*for (int i = cells.Length - 1; i > (cells.Length - piecesCount); i--)
        {
            Debug.LogError("a");
            Instantiate(pieces[_levelNumber].gameObject, cells[i].transform.position, chesspieceRotation);
        }*/
    }

    private void MovePlayer(Transform newPlayerPosition)
    {
        chessPlayer.MoveTo(newPlayerPosition);
    }

    private void MakeMove()
    {

    }
}
