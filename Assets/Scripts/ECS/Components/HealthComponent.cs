﻿using System;
using ECS.Untils;
using Scellecs.Morpeh;

namespace ECS.Components
{
    [Serializable]
    public struct HealthComponent : IComponent, ICloneable
    {
        public int HealthCurrent;
        public int HealthMax;
        public bool IsLive => HealthCurrent > 0;
        public int HealthPerTick;
        public Timer Timer;
        
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}