using ECS.Components;
using ECS.Factories;
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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(HealthSystem))]
    public sealed class HealthSystem : UpdateSystem
    {

        private Filter _filter;
        
        public override void OnAwake() 
        {
            World = WorldManager.WorldDefault;
            _filter = World.Filter
                .With<HealthComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in _filter)
            {
                ref var healthComponent = ref entity.GetComponent<HealthComponent>();
                if (healthComponent.IsLive == false || healthComponent.HealthPerTick == 0)
                {
                    if (entity.Has<DeactivateColliderOnDeadComponent>())
                    {
                        // отключаем визическую оболочку
                        ref var deactivateColliderOnDeadComponent = ref entity.GetComponent<DeactivateColliderOnDeadComponent>();
                        deactivateColliderOnDeadComponent.Deactivate();
                        entity.RemoveComponent<DeactivateColliderOnDeadComponent>();
                    }

                    continue;
                }

                

                if (healthComponent.Timer.UpdateAndCheck(deltaTime))
                {
                    HealFactory.CreateHealRequest(entity,healthComponent.HealthPerTick, HealType.Regeneration);
                    healthComponent.Timer.ResetTimer();
                }
            }
        }
    }
}

