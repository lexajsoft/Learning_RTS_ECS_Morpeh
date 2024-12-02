using ECS.Components;
using ECS.Untils;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(HealProcessingSystem))]
    public sealed class HealProcessingSystem : UpdateSystem
    {
        private Filter _filter;
        private World _world;
    
        public override void OnAwake()
        {
            _world = WorldManager.GetWorld(WorldManager.WORLD_EVENT);
            _filter = _world.Filter
                .With<HealRequestComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime) 
        {
            // пока что это заготовка для хиллеров, если конечно решусь их реализовывать
            
            foreach (var entity in _filter)
            {
                ref var healRequestComponent = ref entity.GetComponent<HealRequestComponent>();
                if (healRequestComponent.Target != null && healRequestComponent.Target.Has<HealthComponent>())
                {
                    ref var healthComponent = ref healRequestComponent.Target.GetComponent<HealthComponent>();
                    healthComponent.Heal(healRequestComponent.Heal);
                
                    _world.RemoveEntity(entity);
                }
                else
                {
                    _world.RemoveEntity(entity);
                }
            }
        }
    }
}