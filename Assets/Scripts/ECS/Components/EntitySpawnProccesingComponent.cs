using System;
using ECS.ScriptableObjects;
using Scellecs.Morpeh;

namespace ECS.Components
{
    [Serializable]
    public struct EntitySpawnProccesingComponent : IComponent
    {
        public float Remaing;
        public EntityDescriptionScriptableObject entityDescriptionScriptableObject;

        public EntitySpawnProccesingComponent(EntityDescriptionScriptableObject NpcEntityDescriptionScriptableObject)
        {
            entityDescriptionScriptableObject = NpcEntityDescriptionScriptableObject;
            Remaing = entityDescriptionScriptableObject.GetEntityDescriptionData().CreationTime;
        }
    }
}