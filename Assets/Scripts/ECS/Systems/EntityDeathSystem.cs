using ECS.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EntityDeathSystem))]
    public sealed class EntityDeathSystem : UpdateSystem
    {
        private Filter _filter;

        public override void OnAwake()
        {
            World = WorldManager.WorldDefault;
            _filter = World.Filter
                .With<HealthComponent>()
                .Without<DestroyAfterComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var healthComponent = ref entity.GetComponent<HealthComponent>();
                if (healthComponent.IsLive == false)
                {
                    // entity.RemoveComponent<HealthComponent>();
                    // if (entity.Has<AnimationComponent>())
                    // {
                    //     ref var animationComponent = ref entity.GetComponent<AnimationComponent>();
                    //     animationComponent.Animator.Play("Death");
                    // }

                    if (entity.Has<TransformComponent>())
                    {
                        ref var transforComponent = ref entity.GetComponent<TransformComponent>();
                        entity.SetComponent(new DestroyAfterComponent(5f, transforComponent.Transform.gameObject));
                    }
                    else
                    {
                        entity.SetComponent(new DestroyAfterComponent(5f, null));
                    }

                }
            }
        }
    }
}
