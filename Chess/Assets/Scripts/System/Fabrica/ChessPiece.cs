using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour, IEntity
{
    [Header("Stats")]
    [SerializeField] private float fullHealth;
    [SerializeField] private float attackStreght;
    [SerializeField] private float deathDelay;

    private float _currentHealth;
    public int CurrentX;
    public int CurrentY;
    private Action onGettingDamage;

    public void Create()
    {
        onGettingDamage += Kill;
    }

    public void Kill()
    {
        StartCoroutine(DeathCooldown());
    }
    private IEnumerator DeathCooldown()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(this.gameObject);
    }

    public void Spawn(ChessBoardCell cell)
    {
        _currentHealth = fullHealth;
        CurrentX = cell.CellToIntCoordinates()[0];
        CurrentY = cell.CellToIntCoordinates()[1];
        var chesspieceRotation = new Quaternion(0, 180, 0, 0);

        Instantiate(gameObject, cell.transform.position, chesspieceRotation);
    }

    public void Attack()
    {
        //Player.Instance.GetDamage(attackStreght);
    }

    public void GetDamage(float damage)
    {
        _currentHealth =- damage;
        onGettingDamage?.Invoke();
    }
}
