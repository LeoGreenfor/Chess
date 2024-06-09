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

    public ChessBoardCell CurrentCell;
    public int CurrentX;
    public int CurrentY;

    private float _currentHealth;
    private Action onGettingDamage;
    private Player _player;

    public void Create()
    {
        onGettingDamage += Kill;
    }
    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void Retreat()
    {

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

    public EntityController Spawn(ChessBoardCell cell)
    {
        _currentHealth = fullHealth;
        CurrentCell = cell;
        CurrentX = cell.CellToIntCoordinates()[0];
        CurrentY = cell.CellToIntCoordinates()[1];
        var chesspieceRotation = new Quaternion(0, 180, 0, 0);

        var entity = Instantiate(gameObject, cell.transform.position, chesspieceRotation);

        return entity.GetComponent<ChessPieceController>();
    }

    public void Attack(ChessBoardCell cell)
    {
        _player.GetDamage(attackStreght);
    }

    public void GetDamage(float damage)
    {
        _currentHealth =- damage;
        if (_currentHealth <= 0) onGettingDamage?.Invoke();
    }
    public float GetAfterAttackHealth(float damage)
    {
        return _currentHealth - damage;
    }

    public float GetStrength()
    {
        return attackStreght;
    }
}
