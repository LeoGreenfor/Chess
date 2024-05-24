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

    [Header("Movements")]
    [SerializeField] private int goToX;
    [SerializeField] private int goToY;
    [SerializeField] private bool isMoveByStraight;
    [SerializeField] private bool isMoveByDiagonal;

    private float _currentHealth;
    private int _currentX;
    private int _currentY;
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
        _currentX = cell.CellToIntCoordinates()[0];
        _currentY = cell.CellToIntCoordinates()[1];
    }

    public void Attack()
    {
        Player.Instance.GetDamage(attackStreght);
    }

    public void GetDamage(float damage)
    {
        _currentHealth =- damage;
        onGettingDamage?.Invoke();
    }
}
