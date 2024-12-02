using System;
using UnityEngine;

namespace ECS.Factories.Data
{
    [Serializable]
    public struct VisualEffectData
    {
        public GameObject Prefab;
        public float LiveTime;
        public AttackType AttackType;
    }
}