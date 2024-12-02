using System;
using Scellecs.Morpeh;

namespace ECS.Components
{
    [Serializable]
    public struct TargetComponent : IComponent
    {
        public Entity Target;
    }
}