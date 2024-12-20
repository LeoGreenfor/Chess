﻿using UnityEngine;

namespace LevelGeneratorRelated.Scripts
{
    public class DeadEnd : MonoBehaviour
    {
        /// <summary>
        /// Bounds node in hierarchy
        /// </summary>
        public Bounds Bounds;

        public void Initialize(LevelGenerator levelGenerator)
        {
            transform.SetParent(levelGenerator.Container);
            levelGenerator.RegistrerNewDeadEnd(Bounds.Colliders);
        }
    }
}