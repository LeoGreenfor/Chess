using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IEntity))]
public abstract class EntityController : MonoBehaviour
{
    private IEntity _Entity;

    private void SpawnEntity(ChessBoardCell cell)
    {
        _Entity.Spawn(cell);
    }

    protected abstract void CreateEntity();

    protected abstract void Retreat();

    protected abstract void MoveTo();
}
