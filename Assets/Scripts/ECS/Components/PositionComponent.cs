using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct PositionComponent : IComponent
    {
        public Vector3 Pos;
    }
}