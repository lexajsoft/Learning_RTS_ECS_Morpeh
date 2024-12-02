using ECS.Components;
using Scellecs.Morpeh;

namespace ECS.Factories
{
    public static class FindNearTargetRequestFactory
    {
        public static void CreateRequest(Entity targetRequest, TargetType findTargetType)
        {
            var entity = WorldManager.WorldEvent.CreateEntity();
            entity.SetComponent(new FindNearTargetEntityRequestComponent()
            {
                Entity = targetRequest,
                FindTargetType = findTargetType
            });
        }
    }
}