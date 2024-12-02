using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct DeactivateColliderOnDeadComponent : IComponent
    {
        public Rigidbody Rigidbody;
        public Collider Collider;
        
        public void Deactivate()
        {
            Collider.isTrigger = true;
            Rigidbody.isKinematic = true;
        }
    }
}