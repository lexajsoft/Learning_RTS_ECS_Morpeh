using System;
using Scellecs.Morpeh;

namespace ECS.Components
{
    [Serializable]
    public struct MainTargetComponent : IComponent
    {
        public Entity Target;
    }
}