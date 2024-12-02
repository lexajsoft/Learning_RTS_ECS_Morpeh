using System;
using Scellecs.Morpeh;
using UnityEngine.Serialization;

namespace ECS.Components
{
    public enum HealType
    {
        Regeneration,
        Heal
    }

    [Serializable]
    public struct HealRequestComponent : IComponent
    {
        public Entity Target;
        public int Heal;
        public HealType HealType;
    }
    
}