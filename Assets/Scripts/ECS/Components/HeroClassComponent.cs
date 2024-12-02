using System;
using Scellecs.Morpeh;

namespace ECS.Components
{
    [Serializable]
    public enum HeroClass
    {
        Warrior,
        Archer,
        Healer
    }

    [Serializable]
    public struct HeroClassComponent : IComponent
    {
        public HeroClass HeroClass;
    }
}