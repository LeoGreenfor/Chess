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
    public int Level { get; private set; }

    private float _currentHealth;
    public int CurrentX;
    public int CurrentY;
    private Action onGettingDamage;

    public void Attack(ChessBoardCell cell)
    {
    }
    public string GetPlayerInfo()
    {
        var levelName = "";

        if (Level == 1) levelName = "Pawn";
        if (Level == 2) levelName = "Rook";
        if (Level == 3) levelName = "Bishop";
        if (Level == 4) levelName = "Queen";

        return $"Level: {levelName}\nHealth: {_currentHealth}\nStrength: {Strength}";
    }

    /// <summary>
    /// Creates Player with Lv1 - pawn
    /// </summary>
    public void Create()
    {
        FullHealth = 10f;
        Strength = 5f;
        Defence = 5f;
        Level = 1;
    }
    public void SetStats(float fullHealth, float strength, float defence, Transform transform)
    {
        FullHealth = fullHealth;
        Strength = strength;
        Defence = defence;
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;

        _currentHealth = fullHealth;
    }
    public void SetStats(float fullHealth, float strength, float defence)
    {
        FullHealth = fullHealth;
        Strength = strength;
        Defence = defence;

        _currentHealth = fullHealth;
    }

    public void UpdateLevel(int level)
    {
        Level = level;
    }

    public void Destroy()
    {
        throw new NotImplementedException();
    }

    public void GetDamage(float damage)
    {
        throw new NotImplementedException();
    }

    public void Kill()
    {
        throw new NotImplementedException();
    }

    public void Retreat()
    {
    }

    public EntityController Spawn(ChessBoardCell cell)
    {
        _currentHealth = FullHealth;
        CurrentX = cell.CellToIntCoordinates()[0];
        CurrentY = cell.CellToIntCoordinates()[1];

        var entity = Instantiate(gameObject, cell.transform.position, Quaternion.identity);

        return entity.GetComponent<PlayerController>();
    }
    public float GetAfterAttackHealth(float damage)
    {
        return _currentHealth - damage;
    }
    public float GetStrength()
    {
        return Strength;
    }
}
