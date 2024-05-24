using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    void Create();
    void Spawn(ChessBoardCell cell);
    void Kill();
    void Attack();
    void GetDamage(float damage);
}
