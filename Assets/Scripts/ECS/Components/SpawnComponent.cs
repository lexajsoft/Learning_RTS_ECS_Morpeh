using System;
using System.Collections.Generic;
using ECS.Components.Interface;
using ECS.ScriptableObjects;
using Scellecs.Morpeh;
using UnityEngine.Serialization;

namespace ECS.Components
{
    [Serializable]
    public struct SpawnComponent : IComponent, IComponentEmptyInitting, ICloneable
    {
        public int UnitQuickSpawnCount;
        public List<EntityDescriptionScriptableObject> PoolEntitys;
        public List<EntityDescriptionScriptableObject> QueueEntity;
        
        public void Init()
        {
            QueueEntity = new List<EntityDescriptionScriptableObject>();
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}