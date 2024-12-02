using System;
using Scellecs.Morpeh;

namespace ECS.Components
{
    [Serializable]
    public struct HealerComponent : IComponent
    {
        public float DistanceHeal;
        public float Heal;
        public float Cooldown;
        public float Remain;
    }
}