using System;
using System.Collections.Generic;
using Scellecs.Morpeh;

namespace ECS.Components
{
    [Serializable]
    public struct NearEntitiesComponent : IComponent
    {
        public List<Entity> Allies;
        public List<Entity> Enemies;
    }
}