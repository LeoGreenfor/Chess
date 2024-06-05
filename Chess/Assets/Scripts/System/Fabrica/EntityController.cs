using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(IEntity))]
public abstract class EntityController : MonoBehaviour
{
    public IEntity _Entity;

    [Header("Movements")]
    [SerializeField] private int goToX;
    [SerializeField] private int goToY;
    [SerializeField] private bool isMoveByStraight;
    [SerializeField] private bool isMoveByDiagonal;
    [SerializeField] protected float moveTime;

    [SerializeField] private bool isOnTurn;

    private void SpawnEntity(ChessBoardCell cell)
    {
        _Entity.Spawn(cell);
    }

    protected abstract void CreateEntity();

    protected virtual void Retreat()
    {
        GetOnTurn(false);
    }

    public virtual void MoveTo(Transform newTransform)
    {
        StartCoroutine(MoveToPosition(newTransform.position, moveTime));
        GetOnTurn(false);
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
