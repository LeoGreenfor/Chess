using Plugins.MissionCore.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : Singleton<Player>, IEntity
{
    [Header("Stats")]
    [SerializeField] private float fullHealth;
    [SerializeField] private float strength;
    [SerializeField] private float defence;

    public void Attack()
    {
        throw new NotImplementedException();
    }

    public void Create()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public void Spawn(int x, int y)
    {
        throw new NotImplementedException();
    }
}
