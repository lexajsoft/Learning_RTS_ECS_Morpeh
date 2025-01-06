using System;
using ECS.Components.Interface;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct DeactivateColliderOnDeadComponent : IComponent , IComponentGameObjectInitting, ICloneable
    {
        public Rigidbody Rigidbody;
        public Collider Collider;
        
        public void Deactivate()
        {
            Collider.isTrigger = true;
            Rigidbody.isKinematic = true;
        }

        public void Init(GameObject gameObject)
        {
            Collider = gameObject.GetComponent<Collider>();
            if (gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                Rigidbody = rigidbody;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}