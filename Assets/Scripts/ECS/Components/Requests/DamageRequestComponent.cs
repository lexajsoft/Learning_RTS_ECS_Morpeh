using System;
using ECS.Factories;
using Scellecs.Morpeh;
using UnityEngine.Serialization;

namespace ECS.Components
{
    [Serializable]
    public struct DamageRequestComponent: IComponent
    {
        public Entity Target;
        public int Damage;
        public AttackType AttackType;
    }
}