using System;
using ECS.Components.Interface;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct TransformComponent : IComponent, IComponentGameObjectInitting, ICloneable
    {
        public Transform Transform;

        public TransformComponent(Transform transform)
        {
            Transform = transform;
        }

        public void Init(GameObject gameObject)
        {
            Transform = gameObject.transform;
        }


        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}