using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour, IEntity
{
    [Header("Stats")]
    [SerializeField]
    private float fullHealth;
    [SerializeField]
    private float attackStreght;

    [Header("Movements")]
    [SerializeField] private int goToX;
    [SerializeField] private int goToY;

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Create()
    {
        throw new System.NotImplementedException();
    }

    public void Destroy()
    {
        throw new System.NotImplementedException();
    }

    public void Kill()
    {
        throw new System.NotImplementedException();
    }

    public void Retreat()
    {
        throw new System.NotImplementedException();
    }

    public void Spawn()
    {
        throw new System.NotImplementedException();
    }
}
