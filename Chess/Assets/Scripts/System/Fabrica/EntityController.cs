using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(IEntity))]
public abstract class EntityController : MonoBehaviour
{
    public IEntity Entity;

    public ChessBoardCell CurrentCell;

    [Header("Movements")]
    [SerializeField] protected int goToX;
    [SerializeField] protected int goToY;
    [SerializeField] protected bool isMoveByStraight;
    [SerializeField] protected bool isMoveByDiagonal;
    [SerializeField] protected float moveTime;

    [SerializeField] private bool isOnTurn;

    public virtual EntityController SpawnEntity(ChessBoardCell cell)
    {
        var entity = Entity.Spawn(cell);
        CurrentCell = cell;

        return entity;
    }
    public void SetCurrentCell(ChessBoardCell cell)
    {
        CurrentCell = cell;
    }

    public virtual void CreateEntity(Player player)
    {
        this.Entity = GetComponent<IEntity>();
    }

    public virtual void MakeMove(PlayerController player)
    {
        SetIsOnTurn(false);
    }

    public virtual void MoveTo(ChessBoardCell newCell)
    {
        Debug.LogError(Entity);
        if (isOnTurn)
        {
            bool isInRadius = IsCorrectCoordinates(newCell);
            Debug.LogError("move to 2");

            if (isInRadius)
            {
                StartCoroutine(MoveToPosition(newCell.transform.position, moveTime));
                CurrentCell = newCell;
                SetIsOnTurn(false);
            }

            newCell.IsOccupied = true;
        }
    }
    private IEnumerator MoveToPosition(Vector3 newPosition, float time)
    {
        Debug.LogError(Entity);
        Debug.LogError("move to position");

        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPosition, newPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = newPosition;
    }

    public void SetIsOnTurn(bool value)
    {
        isOnTurn = value;
    }

    public int[] CorrectCoordinates()
    {
        int[] coordinates = CurrentCell.CellToIntCoordinates();
        var randomValue = Random.Range(0, 10);
        int newX = 0, newY = 0;

        if (goToX <= 1) newX = 1;
            else newX = Random.Range(1, goToX);
        if (goToY <= 1) newY = 1;
        else newY = Random.Range(1, goToY);

        if (isMoveByStraight)
        {
            if (randomValue % 2 == 0) coordinates[0] += newX;
            else coordinates[1] += newY;
        }
        if (isMoveByDiagonal)
        {
            if (randomValue % 2 == 0) coordinates[1] += newY;
            else coordinates[0] += newX;
        }

        return coordinates;
    }
    public bool IsCorrectCoordinates(ChessBoardCell newCell)
    {
        int currentX = CurrentCell.CellToIntCoordinates()[0];
        int currentY = CurrentCell.CellToIntCoordinates()[1];

        int newX = newCell.CellToIntCoordinates()[0];
        int newY = newCell.CellToIntCoordinates()[1];

        int differenceX = Mathf.Abs(newX - currentX);
        int differenceY = Mathf.Abs(newY - currentY);
        bool isInRadiusX = differenceX <= goToX, isInRadiusY = differenceY <= goToY;
        bool isInRadius = false;

        if (currentY == newY)
        {
            if (isInRadiusX && isMoveByStraight)
                isInRadius = true;
        }
        else if (currentX == newX)
        {
            if (isInRadiusY && isMoveByStraight)
                isInRadius = true;
        }
        else if (isInRadiusX && isInRadiusY && isMoveByDiagonal)
            isInRadius = true;

        if (newCell == CurrentCell) isInRadius = false;

        return isInRadius;
    }
}
