using System;
using ECS.Factories;
using ECS.Untils;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct MelleAttackComponent : IComponent
    {
        // стартовое расстояние для начала атаки
        public float StartDistanceAttack;
        // расстояние на котором может наносить удар, может быть немного дальше стартовой
        public float DistanceAttack;
        public Vector2Int Damage;
        public float AttackTime;
        
        public int GetDamage()
        {
            return UnityEngine.Random.Range(Damage.x, Damage.y + 1);
        }
    }
    
    
    [Serializable]
    public struct RangeAttackComponent : IComponent
    {
        // стартовое расстояние для начала атаки
        public float StartDistanceAttack;
        // расстояние на котором может наносить удар, может быть немного дальше стартовой
        public float DistanceAttack;
        public float AttackTime;
    }

    public struct ProjectileComponent : IComponent
    {
        public int Damage;
    }

    [Serializable]
    public struct AttackProcessingComponent : IComponent
    {
        public Entity Target;
        // public int Damage;
        public Timer Timer;
    }
}