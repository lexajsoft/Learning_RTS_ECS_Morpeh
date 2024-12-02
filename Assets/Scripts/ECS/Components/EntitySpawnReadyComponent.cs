using ECS.ScriptableObjects;
using Scellecs.Morpeh;

namespace ECS.Components
{
    public struct EntitySpawnReadyComponent : IComponent
    {
        public EntityDescriptionScriptableObject entityDescriptionScriptableObject;
        public EntitySpawnReadyComponent(EntityDescriptionScriptableObject EntityDescriptionScriptableObject)
        {
            entityDescriptionScriptableObject = EntityDescriptionScriptableObject;
        }
    }
}