using System;
using ECS.Components.Interface;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public enum TagTeam
    {
        None,
        Red,
        Green,
        Blue,
        Purple
    }

    [Serializable]
    public struct TagTeamComponent : IComponent, IComponentGameObjectInitting, ICloneable
    {
        public TagTeam TagTeam;
        public object Clone()
        {
            return MemberwiseClone();
        }

        public void Init(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<ReColoringByTagTeam>(out var reColoringByTagTeam))
            {
                reColoringByTagTeam.ReColor(TagTeam);
            }
        }
    }
}