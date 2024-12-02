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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageProcessingSystem))]
    public sealed class DamageProcessingSystem : UpdateSystem
    {
        private Filter _filter;
        private World _world;
    
        public override void OnAwake()
        {
            _world = WorldManager.GetWorld(WorldManager.WORLD_EVENT);
            _filter = _world.Filter
                .With<DamageRequestComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in _filter)
            {
                ref var damageRequestComponent = ref entity.GetComponent<DamageRequestComponent>();
                if (damageRequestComponent.Target != null && damageRequestComponent.Target.IsDisposed() == false)
                {
                    if (damageRequestComponent.Target.Has<HealthComponent>())
                    {
                        ref var healthComponent = ref damageRequestComponent.Target.GetComponent<HealthComponent>();
                        healthComponent.Damage(damageRequestComponent.Damage);
                    }
                }
                _world.RemoveEntity(entity);
            }
        }
    }
}