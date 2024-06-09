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
        OnPlayerMakeMove += BeginMove;

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
            var entity = chessPlayerControllerPrefab.SpawnEntity(rows[0].cells[1]);
            chessPlayer = entity.GetComponent<PlayerController>();
            
            var player = chessPlayer.gameObject.GetComponent<Player>();
            player.UpdateLevel(_levelNumber);
            chessPlayer.CreateEntity(player);
            chessPlayer.SetCurrentCell(rows[0].cells[1]);


            SetChessPieces();
        }
    }

    private void SetChessPieces()
    {
        int piecesCount = WinsCounts + 1;
        int index = rows.Length - 1;
        piecesOnBoard = new ChessPieceController[piecesCount];
        EntityController entity;

        for (int i = 0; i < piecesCount; i++)
        {
            if (i % 2 == 0) 
            {
                entity = piecesPrefabs[_levelNumber].SpawnEntity(rows[index].cells[i]);
            }
            else
            {
                entity = piecesPrefabs[_levelNumber + 1].SpawnEntity(rows[index].cells[i]);
            }

            piecesOnBoard[i] = entity as ChessPieceController;
            piecesOnBoard[i].SetCurrentCell(rows[index].cells[i]);
            piecesOnBoard[i].CreateEntity(chessPlayer.Entity as Player);
        }
    }

    private void MovePlayer(ChessBoardCell newPlayerPosition)
    {
        chessPlayer.SetIsOnTurn(true);
        chessPlayer.CurrentCell.IsOccupied = false;

        if (!chessPlayer.CurrentCell.Equals(newPlayerPosition))
            OnPlayerMakeMove?.Invoke();

        chessPlayer.MoveTo(newPlayerPosition);
    }

    private void BeginMove()
    {
        StartCoroutine(MakeMove());
    }

    private IEnumerator MakeMove()
    {
        yield return new WaitForSeconds(2f);
        // find nearest chess piece
        /*var minDistance = Vector3.Distance(chessPlayer.transform.position, piecesOnBoard[0].transform.position);
        var nearestPiece = piecesOnBoard[0];

        for (int i = 0;i < piecesOnBoard.Length; i++)
        {
            var distance = Vector3.Distance(chessPlayer.transform.position, piecesOnBoard[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPiece = piecesOnBoard[i];
            }
        }*/

        // find chess piece that can move to player
        ChessPieceController chessPiece = piecesOnBoard[0];
        bool isCanMakeMove = piecesOnBoard[0].IsCorrectCoordinates(chessPlayer.CurrentCell);

        for (int i = 0; i < piecesOnBoard.Length; i++)
        {
            if (piecesOnBoard[i].IsCorrectCoordinates(chessPlayer.CurrentCell))
            {
                chessPiece = piecesOnBoard[i];
                isCanMakeMove = true;
                Debug.LogError(chessPiece.ToString());
                break;
            }
        }

        // if there is any - make move, otherwise - just move
        if (isCanMakeMove || chessPlayer.IsCorrectCoordinates(chessPiece.CurrentCell))
        {
            Debug.LogError("make move");
            chessPiece.SetIsOnTurn(true);
            chessPiece.MakeMove(chessPlayer);
        }
        else
        {
            for (int i = 0; i < rows.Length; i++)
            {
                bool isFindCoordinates = false;
                for (int j = 0; j < rows[i].cells.Length; j++)
                    if (chessPiece.IsCorrectCoordinates(rows[i].cells[j]) && !rows[i].cells[j].IsOccupied)
                    {
                        Debug.LogError("move to 1");
                        chessPiece.SetIsOnTurn(true);
                        chessPiece.CurrentCell.IsOccupied = false;
                        chessPiece.MoveTo(rows[i].cells[j]);
                        isFindCoordinates = true;
                        break;
                    }
                if (isFindCoordinates) break;
            }
        }

    }

    [System.Serializable]
    private class BoardCells
    {
        public ChessBoardCell[] cells;
    }
}
