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
        SetCurrentCell(cell);

        return entity;
    }
    public void SetCurrentCell(ChessBoardCell cell)
    {
        CurrentCell = cell;
        CurrentCell.IsOccupied = true;
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
        if (isOnTurn)
        {
            bool isInRadius = IsCorrectCoordinates(newCell);

            if (isInRadius)
            {
                StartCoroutine(MoveToPosition(newCell.transform.position, moveTime));
                CurrentCell.IsOccupied = false;
                SetCurrentCell(newCell);
                SetIsOnTurn(false);
            }
        }
    }
    private IEnumerator MoveToPosition(Vector3 newPosition, float time)
    {
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
    public bool GetIsOnTurn()
    {
        return isOnTurn;
    }

    public int[] CorrectCoordinates(int borderNumber, int borderLetter)
    {
        int[] coordinates = CurrentCell.CellToIntCoordinates();
        var randomValue = Random.Range(0, 10);
        int newX = 0, newY = 0;

        if (goToX <= 1) newX = 1;
            else newX = Random.Range(1, goToX);
        if (goToY <= 1) newY = 1;
        else newY = Random.Range(1, goToY);

        if (coordinates[0] + newX > borderNumber) newX *= -1;
        if (coordinates[1] + newY > borderLetter) newY *= -1;

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
        bool isInRadius = IsInRadius(newCell);

        /*Debug.LogError($"new cell {newCell}, current cell {CurrentCell};\n" +
            $"is in radius x {isInRadiusX}, is in radius y {isInRadiusY}");*/

        if (newCell == CurrentCell) isInRadius = false;

        return isInRadius;
    }

    public bool IsInRadius(ChessBoardCell newCell)
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

        return isInRadius;
    }
}
