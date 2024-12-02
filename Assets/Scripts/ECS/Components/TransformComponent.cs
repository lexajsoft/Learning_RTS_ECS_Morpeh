using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct TransformComponent : IComponent
    {
        public Transform Transform;

        public TransformComponent(Transform transform)
        {
            Transform = transform;
        }
    }
}