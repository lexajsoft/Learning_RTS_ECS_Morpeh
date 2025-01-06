using System;
using ECS.Components.Interface;
using Scellecs.Morpeh;

namespace ECS.Components
{
    [Serializable]
    public struct TargetComponent : IComponent, IComponentEmptyInitting,ICloneable
    {
        public Entity Target;
        public void Init()
        {
            Target = null;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}