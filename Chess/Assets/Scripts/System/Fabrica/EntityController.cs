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

    public EntityController SpawnEntity(ChessBoardCell cell)
    {
        Entity = GetComponent<IEntity>();

        var entity = Entity.Spawn(cell);
        CurrentCell = cell;

        return entity;
    }
    public void SetCurrentCell(ChessBoardCell cell)
    {
        CurrentCell = cell;
    }

    public abstract void CreateEntity(Player player);

    protected virtual void Retreat()
    {
        SetOnTurn(false);
    }

    public virtual void MoveTo(ChessBoardCell newCell)
    {
        if (isOnTurn)
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

            if (isInRadius)
            {
                StartCoroutine(MoveToPosition(newCell.transform.position, moveTime));
                CurrentCell = newCell;
                SetOnTurn(false);
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

    public void SetOnTurn(bool value)
    {
        isOnTurn = value;
    }

    public int[] CorrectCoordinates()
    {
        int[] coordinates = new int[2];
        coordinates[0] = CurrentCell.CellToIntCoordinates()[0] + Random.Range(1, goToX);
        coordinates[1] = CurrentCell.CellToIntCoordinates()[1] + Random.Range(1, goToY);

        return coordinates;
    }
}
