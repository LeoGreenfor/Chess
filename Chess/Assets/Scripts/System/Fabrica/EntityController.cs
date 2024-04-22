using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    private void SpawnEntity() { }

    protected abstract void CreateEntity();
}
