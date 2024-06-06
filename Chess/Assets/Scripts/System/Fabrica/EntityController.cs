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
    [SerializeField] private int goToX;
    [SerializeField] private int goToY;
    [SerializeField] private bool isMoveByStraight;
    [SerializeField] private bool isMoveByDiagonal;
    [SerializeField] protected float moveTime;

    [SerializeField] private bool isOnTurn;

    public void SpawnEntity(ChessBoardCell cell)
    {
        Entity = GetComponent<IEntity>();
        Entity.Spawn(cell);
        CurrentCell = cell;
    }

    protected abstract void CreateEntity();

    protected virtual void Retreat()
    {
        GetOnTurn(false);
    }

    public virtual void MoveTo(ChessBoardCell newCell)
    {
        int currentX = CurrentCell.CellToIntCoordinates()[0];
        int currentY = CurrentCell.CellToIntCoordinates()[1];

        int newX = newCell.CellToIntCoordinates()[0];
        int newY = newCell.CellToIntCoordinates()[1];

        int differenceX = Mathf.Abs(newX - currentX);
        int differenceY = Mathf.Abs(newY - currentY);
        bool isInRadiusX = differenceX <= goToX, isInRadiusY = differenceY <= goToY;

        if ((isMoveByStraight && (isInRadiusX && currentY == newY)) // move horizontally
            || ((isMoveByStraight && (currentX == newX && isInRadiusY))) // move vertically
            || (isMoveByDiagonal && isInRadiusX && isInRadiusY && currentY != newY && currentX != newX)) // move by diagonal
        {
            StartCoroutine(MoveToPosition(newCell.transform.position, moveTime));
            GetOnTurn(false);
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

    protected void GetOnTurn(bool value)
    {
        isOnTurn = value;
    }
}
