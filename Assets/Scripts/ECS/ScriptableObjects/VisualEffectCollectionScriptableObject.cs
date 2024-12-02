using System.Collections.Generic;
using ECS.Factories.Data;
using UnityEngine;

namespace ECS.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create/VisualEffectCollectionScriptableObject", fileName = "VisualEffectCollectionScriptableObject", order = 0)]
    public class VisualEffectCollectionScriptableObject : ScriptableObject
    {
        public List<VisualEffectData> List;
    }
}