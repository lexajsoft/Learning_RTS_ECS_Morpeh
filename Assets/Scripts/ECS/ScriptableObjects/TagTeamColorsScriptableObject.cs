using System.Collections.Generic;
using ECS.Components;
using ECS.Factories.Data;
using UnityEngine;

namespace ECS.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create/TagTeamColorsScriptableObject", fileName = "TagTeamColorsScriptableObject", order = 0)]
    public class TagTeamColorsScriptableObject : ScriptableObject
    {
        [SerializeField] private List<TagTeamColorData> _colors;

        public Color GetColorByTagTeam(TagTeam tagTeam)
        {
            var index = _colors.FindIndex(item => item.TagTeam == tagTeam);
            if (index >= 0)
            {
                return _colors[index].Color;
            }
            return Color.cyan;
        }
    }
}