using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    void Create();
    void Destroy();
    void Spawn();
    void Kill();
    void Attack();
    void Retreat();
}
