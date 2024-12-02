using System;
using Scellecs.Morpeh;

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
    public struct TagTeamComponent : IComponent
    {
        public TagTeam TagTeam;
    }
}