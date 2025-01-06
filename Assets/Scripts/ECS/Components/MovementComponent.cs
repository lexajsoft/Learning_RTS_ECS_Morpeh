using System;
using ECS.Components.Interface;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct MovementComponent : IComponent, IComponentGameObjectInitting, ICloneable
    {
        public Rigidbody Rigidbody;
        public Transform Transform;
        public Vector3 Direct;
        public float Speed;
        
        
        public void Init(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<Rigidbody>(out var rigidbody)) return;
            
            Transform = gameObject.transform;
            Rigidbody = rigidbody;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}