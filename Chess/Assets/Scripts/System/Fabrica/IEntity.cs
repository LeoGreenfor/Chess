using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    bool IsKilled();
    void Create();
    EntityController Spawn(ChessBoardCell cell);
    void Kill();
    void Attack(IEntity entity);
    void GetDamage(float damage);
    void Retreat();
    float GetAfterAttackHealth(float damage);
    float GetStrength();
}
