using System;
using ECS.Components.Interface;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct PositionComponent : IComponent, IComponentGameObjectInitting, ICloneable
    {
        public Vector3 Pos;
        public void Init(GameObject gameObject)
        {
            Pos = gameObject.transform.position;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}