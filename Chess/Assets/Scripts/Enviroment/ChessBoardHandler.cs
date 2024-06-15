using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameGeneralSettings;

public class ChessBoardHandler : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private BoardCells[] rows;

    [SerializeField] private ChessPieceController[] piecesPrefabs;
    [SerializeField] private ChessPieceController[] piecesOnBoard;

    [SerializeField] private PlayerController chessPlayerControllerPrefab;
    [SerializeField] private PlayerController chessPlayer;

    [Header("Settings")]
    [SerializeField] private Camera boardCamera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text PlayerInfo;
    [SerializeField] private TMP_Text BoardInfo;

    public Action<bool> OnGameStateChange;
    public Action OnPlayerMakeMove;

    public bool IsGameBegin {  get; private set; }

    private int _levelNumber;
    private string _levelName;
    private int _boardSideToInt;

    private void Start()
    {
        OnGameStateChange += SetGameState;
        OnPlayerMakeMove += BeginMove;

        _boardSideToInt = GameManager.Instance.MainChessSideIndex;

        ChessPiece[] chessPieces = GameManager.Instance.generalSettings.chessPiecesPrefabs[_boardSideToInt].chessPieces;
        piecesPrefabs = new ChessPieceController[chessPieces.Length];

        for (int i = 0; i < chessPieces.Length; i++)
        {
            piecesPrefabs[i] = chessPieces[i].GetComponent<ChessPieceController>();
        }
        
        _levelNumber = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < rows.Length; i++)
            for (int j = 0; j < rows[i].cells.Length; j++)
                rows[i].cells[j].OnPlayerMove += MovePlayer;
    }

    private void SetGameState(bool state)
    {
        IsGameBegin = state;
        boardCamera.gameObject.SetActive(IsGameBegin);
        canvas.gameObject.SetActive(IsGameBegin);

        if (IsGameBegin)
        {
            var entity = chessPlayerControllerPrefab.SpawnEntity(rows[0].cells[1]);
            chessPlayer = entity.GetComponent<PlayerController>();
            
            var player = chessPlayer.gameObject.GetComponent<Player>();
            player.UpdateLevel(_levelNumber);
            chessPlayer.CreateEntity(player);
            chessPlayer.SetCurrentCell(rows[0].cells[1]);


            SetChessPieces();

            PlayerInfo.text = chessPlayer.GetComponent<Player>().GetPlayerInfo();
            BoardInfo.text = GetBoardInfo();

            chessPlayer.SetIsOnTurn(true);
        }
        else
        {
            for (int i = 0;i < piecesOnBoard.Length; i++)
                Destroy(piecesOnBoard[i].gameObject);

            piecesOnBoard = new ChessPieceController[0];
            Destroy(chessPlayer.gameObject);
        }
    }

    private void SetChessPieces()
    {
        int piecesCount = GameManager.Instance.LevelHandler.WinsCounts + 1;
        int index = rows.Length - 1;
        piecesOnBoard = new ChessPieceController[piecesCount];
        EntityController entityController;

        for (int i = 0; i < piecesCount; i++)
        {
            if (i % 2 == 0) 
            {
                entityController = piecesPrefabs[_levelNumber - 1].SpawnEntity(rows[index].cells[i]);
            }
            else
            {
                entityController = piecesPrefabs[_levelNumber].SpawnEntity(rows[index].cells[i]);
            }

            piecesOnBoard[i] = entityController as ChessPieceController;
            piecesOnBoard[i].SetCurrentCell(rows[index].cells[i]);
            piecesOnBoard[i].CreateEntity(chessPlayer.Entity as Player);

            var entity = piecesOnBoard[i].Entity as ChessPiece;
        }
    }

    private void MovePlayer(ChessBoardCell newPlayerPosition)
    {
        if (!chessPlayer.CurrentCell.Equals(newPlayerPosition) && chessPlayer.GetIsOnTurn())
        {
            bool isAttacking = false;

            for (int i = 0; i < piecesOnBoard.Length; i++)
            {
                if (piecesOnBoard[i].IsInRadius(newPlayerPosition) && newPlayerPosition.IsOccupied)
                {
                    isAttacking = true;
                    chessPlayer.Entity.Attack(piecesOnBoard[i].Entity);
                    OnPlayerMakeMove?.Invoke();
                    break;
                }
            }

            if (!isAttacking && !newPlayerPosition.IsOccupied)
            {
                chessPlayer.MoveTo(newPlayerPosition);
                OnPlayerMakeMove?.Invoke();
            }
        }
    }

    private void BeginMove()
    {
        StartCoroutine(MakeMove());
    }

    private IEnumerator MakeMove()
    {
        yield return new WaitForSeconds(2f);

        // find chess piece that can move to player
        ChessPieceController chessPiece = null;
        for (int i = 0; i < piecesOnBoard.Length; i++)
            if (!piecesOnBoard[i].Entity.IsKilled())
            {
                chessPiece = piecesOnBoard[i];
                break;
            }

        if (chessPiece == null) GameManager.Instance.OnMatchEnd?.Invoke(true);
        else
        {

            bool isCanMakeMove = piecesOnBoard[0].IsCorrectCoordinates(chessPlayer.CurrentCell);

            for (int i = 0; i < piecesOnBoard.Length; i++)
            {
                if (piecesOnBoard[i].IsCorrectCoordinates(chessPlayer.CurrentCell))
                {
                    chessPiece = piecesOnBoard[i];
                    isCanMakeMove = true;
                    break;
                }
            }

            // if there is any - make move, otherwise - just move
            if (isCanMakeMove || chessPlayer.IsCorrectCoordinates(chessPiece.CurrentCell))
            {
                Debug.LogError("make move");
                chessPiece.SetIsOnTurn(true);
                chessPiece.MakeMove(chessPlayer);

                var entity = chessPiece.Entity as ChessPiece;
                if (entity.IsRetreating)
                {
                    Debug.LogError("a");
                    chessPiece.SetIsOnTurn(true);
                    chessPiece.MoveTo(FindUnoccupiedCell(chessPiece));
                }
            }
            else
            {
                chessPiece.SetIsOnTurn(true);
                chessPiece.MoveTo(FindUnoccupiedCell(chessPiece));
            }

            Debug.LogError(chessPiece.GetComponent<ChessPiece>().GetCurrentHealth());
            chessPlayer.SetIsOnTurn(true);
        }

        PlayerInfo.text = chessPlayer.GetComponent<Player>().GetPlayerInfo();
        BoardInfo.text = GetBoardInfo();
    }

    private ChessBoardCell FindUnoccupiedCell(ChessPieceController chessPiece)
    {
        bool isFindCoordinates = false;
        ChessBoardCell chessBoardCell = null;

        while (!isFindCoordinates)
        {
            var newCoo = chessPiece.CorrectCoordinates(rows.Length, rows[0].cells.Length);

            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].cells.Length; j++)
                {
                    if ((rows[i].cells[j].CellToIntCoordinates()[0] == newCoo[0])
                        && (rows[i].cells[j].CellToIntCoordinates()[1] == newCoo[1])
                        && !rows[i].cells[j].IsOccupied)
                    {
                        chessBoardCell = rows[i].cells[j];
                        isFindCoordinates = true;
                        break;
                    }
                }

                if (isFindCoordinates) break;
            }
        }
        
        return chessBoardCell;
    }

    private string GetBoardInfo()
    {
        return $"The board\nChess pieces left: {piecesOnBoard.Length}\nLevel name: {_levelName}";
    }
    public void SetLevelName(string levelName) => _levelName = levelName;

    [System.Serializable]
    private class BoardCells
    {
        public ChessBoardCell[] cells;
    }
}
