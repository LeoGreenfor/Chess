using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    void Create();
    EntityController Spawn(ChessBoardCell cell);
    void Kill();
    void Attack(ChessBoardCell cell);
    void GetDamage(float damage);
    void Retreat();
    float GetAfterAttackHealth(float damage);
    float GetStrength();
}
