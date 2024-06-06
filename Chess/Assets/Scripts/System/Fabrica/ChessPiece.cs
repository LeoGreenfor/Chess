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
        CurrentX = cell.CellToIntCoordinates()[0];
        CurrentY = cell.CellToIntCoordinates()[1];
        var chesspieceRotation = new Quaternion(0, 180, 0, 0);

        var entity = Instantiate(gameObject, cell.transform.position, chesspieceRotation);

        return entity.GetComponent<ChessPieceController>();
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
