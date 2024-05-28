using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IEntity))]
public abstract class EntityController : MonoBehaviour
{
    private IEntity _Entity;

    [Header("Movements")]
    [SerializeField] private int goToX;
    [SerializeField] private int goToY;
    [SerializeField] private bool isMoveByStraight;
    [SerializeField] private bool isMoveByDiagonal;

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

    protected virtual void MoveTo()
    {
        GetOnTurn(false);
    }

    protected void GetOnTurn(bool value)
    {
        isOnTurn = value;
    }
}
