using ECS.Components;
using Scellecs.Morpeh;

namespace ECS.Factories
{
    public static class DamageFactory
    {
        public static void CreateDamageRequest(Entity target, int damage, AttackType attackType)
        {
            var entity = WorldManager.GetWorld(WorldManager.WORLD_EVENT).CreateEntity();
            
            //var entity = World.Default.CreateEntity();
            entity.SetComponent(new DamageRequestComponent()
            {
                Target = target,
                Damage = damage,
                AttackType = attackType
            });
        }
    }
}