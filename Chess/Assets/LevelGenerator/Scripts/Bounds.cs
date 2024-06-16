using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneratorRelated.Scripts
{
    public class Bounds : MonoBehaviour
    {
            public IEnumerable<Collider> Colliders => GetComponentsInChildren<Collider>();
    }
}
