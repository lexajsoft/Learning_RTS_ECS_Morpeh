using System;
using ECS.Factories;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    // колчан
    [Serializable]
    public struct QuiverComponent : IComponent, ICloneable
    {
        public ProjectileData ProjectileData;
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}