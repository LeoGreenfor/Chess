using Plugins.MissionCore.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour, IEntity
{
    public float FullHealth {  get; private set; }
    public float Strength { get; private set; }
    public float Defence { get; private set; }
    public int Level {
        get {  return _level; } 
        private set 
        {
            _level = value;
            if (_level == 1)
            {
                FullHealth = 10f;
                Strength = 5;
                Defence = 5;
                _levelName = "Pawn";
            }
            if (_level == 2)
            {
                FullHealth = 25f;
                Strength = 10;
                Defence = 10;
                _levelName = "Rook";
            }
            if (_level == 3)
            {
                FullHealth = 75f;
                Strength = 20;
                Defence = 15;
                _levelName = "Bishop";
            }
            if (_level == 4)
            {
                FullHealth = 100f;
                Strength = 25;
                Defence = 15;
                _levelName = "Queen";
            }
        }
    }
    private int _level;
    private string _levelName;

    private float _currentHealth;
    private Action onGettingDamage;
    private float _deathDelay = 2f;
    private bool _isKilled;

    public void Attack(IEntity piece)
    {
        piece.GetDamage(Strength);
    }

    public string GetPlayerInfo()
    {
        return $"Level: {_levelName}\nHealth: {_currentHealth}\nStrength: {Strength}";
    }

    /// <summary>
    /// Creates Player with Lv1 - pawn
    /// </summary>
    public void Create()
    {
        Level = 1;
    }
    public void SetStats(int level, Transform transform)
    {
        Level = level;

        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;

        _currentHealth = FullHealth;
    }
    public void SetStats(int level)
    {
        Level = level;

        _currentHealth = FullHealth;
    }

    public void UpdateLevel(int level)
    {
        Level = level;
    }

    public void Destroy()
    {
    }

    public void GetDamage(float damage)
    {
        _currentHealth -= damage - (damage * Defence * .1f);

        if (_currentHealth <= 0) Kill();
    }

    public void Kill()
    {
        StartCoroutine(DeathCooldown());
    }
    private IEnumerator DeathCooldown()
    {
        yield return new WaitForSeconds(_deathDelay);

        _isKilled = true;
        GameManager.Instance.OnMatchEnd(false);
        gameObject.SetActive(false);
    }

    public void Retreat()
    {
    }

    public EntityController Spawn(ChessBoardCell cell)
    {
        _currentHealth = FullHealth;

        var entity = Instantiate(gameObject, cell.transform.position, Quaternion.identity);

        return entity.GetComponent<PlayerController>();
    }
    public float GetAfterAttackHealth(float damage) => _currentHealth - damage;
    public float GetStrength() => Strength;
    public bool IsKilled() => _isKilled;
    public void RestoreHealth(float health) => _currentHealth = health;

    private void SetLevelStats(int level)
    {
        Level = level;
        if (level == 1)
        {
            FullHealth = 10f;
            Strength = 5;
            Defence = 5;
        }
        if (level == 2)
        {
            FullHealth = 25f;
            Strength = 10;
            Defence = 10;
        }
        if (level == 3)
        {
            FullHealth = 75f;
            Strength = 20;
            Defence = 15;
        }
        if (level == 4)
        {
            FullHealth = 100f;
            Strength = 25;
            Defence = 15;
        }
    }
}
