using ECS.Components;
using Scellecs.Morpeh;

namespace ECS.Factories
{
    public static class UpdateNearRequestFactory
    {
        public static void CreateRequest(Entity entityRequest)
        {
            var entity = WorldManager.WorldEvent.CreateEntity();
            entity.SetComponent(new UpdateNearEntityRequestComponent()
            {
                Entity = entityRequest
            });
        }
    }
}