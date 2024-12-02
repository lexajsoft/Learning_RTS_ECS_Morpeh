using System;
using System.Collections.Generic;
using ECS.ScriptableObjects;
using Scellecs.Morpeh;
using UnityEngine.Serialization;

namespace ECS.Components
{
    [Serializable]
    public struct SpawnComponent : IComponent
    {
        public int UnitQuickSpawnCount;
        public List<EntityDescriptionScriptableObject> PoolEntitys;
        public List<EntityDescriptionScriptableObject> QueueEntity;
    }
}