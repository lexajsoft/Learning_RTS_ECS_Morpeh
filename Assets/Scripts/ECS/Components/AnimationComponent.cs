using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct AnimationComponent : IComponent
    {
        public Animator Animator;
    }
}