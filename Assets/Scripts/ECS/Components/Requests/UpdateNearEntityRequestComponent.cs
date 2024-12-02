using System;
using Scellecs.Morpeh;

namespace ECS.Components
{
    [Serializable]
    public struct UpdateNearEntityRequestComponent : IComponent
    {
        public Entity Entity;
    }

    
}