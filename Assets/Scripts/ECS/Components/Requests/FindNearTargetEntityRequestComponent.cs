using System;
using Scellecs.Morpeh;

namespace ECS.Components
{
    [Serializable]
    public struct FindNearTargetEntityRequestComponent : IComponent
    {
        public Entity Entity;
        public TargetType FindTargetType;
    }
}