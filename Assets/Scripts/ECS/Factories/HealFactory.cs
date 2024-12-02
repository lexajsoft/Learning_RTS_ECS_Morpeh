using ECS.Components;
using Scellecs.Morpeh;

namespace ECS.Factories
{
    public static class HealFactory
    {
        public static void CreateHealRequest(Entity target, int heal, HealType healType)
        {
            var entity = WorldManager.GetWorld(WorldManager.WORLD_EVENT).CreateEntity();
            
            //var entity = World.Default.CreateEntity();
            entity.SetComponent(new HealRequestComponent()
            {
                Target = target,
                Heal = heal,
                HealType = healType
            });
        }
    }
}