using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct MovementComponent : IComponent
    {
        public Rigidbody Rigidbody;
        public Transform Transform;
        public Vector3 Direct;
        public float Speed;
    }
}