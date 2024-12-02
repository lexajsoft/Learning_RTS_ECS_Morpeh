using System;
using ECS.Components;
using UnityEngine;

namespace ECS.Factories.Data
{
    [Serializable]
    public struct TagTeamColorData
    {
        public TagTeam TagTeam;
        public Color Color;
    }
}