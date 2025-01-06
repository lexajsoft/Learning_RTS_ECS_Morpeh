using System;
using System.Collections.Generic;
using Scellecs.Morpeh;

namespace ECS.Components
{
    [Serializable]
    public struct NearEntitiesComponent : IComponent, ICloneable
    {
        public List<Entity> Allies;
        public List<Entity> Enemies;
        public object Clone()
        {
            NearEntitiesComponent copy = new NearEntitiesComponent();
            copy.Allies = new List<Entity>();
            copy.Enemies = new List<Entity>();
            return copy;
        }
    }
}