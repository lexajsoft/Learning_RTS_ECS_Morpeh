using System;
using ECS.Components.Interface;
using ECS.Untils;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct HealthBarComponent : IComponent
    {
        public ProgressBar ProgressBar;
        public Vector3 VisualOffset;
    }
}