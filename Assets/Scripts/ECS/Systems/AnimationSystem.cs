using ECS;
using ECS.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(AnimationSystem))]
public sealed class AnimationSystem : UpdateSystem
{
    private Filter _filter;
    public override void OnAwake()
    {
        World = WorldManager.WorldDefault;
        _filter = World.Filter
            .With<AnimationComponent>()
            .With<HealthComponent>()
            .With<MovementComponent>()
            .Build();
    }

    public override void OnUpdate(float deltaTime) 
    {
        foreach (var entity in _filter)
        {
            ref var animationComponent = ref entity.GetComponent<AnimationComponent>();
            ref var healthComponent = ref entity.GetComponent<HealthComponent>();
            ref var movementComponent = ref entity.GetComponent<MovementComponent>();

            if (entity.Has<AttackProcessingComponent>())
            {
                animationComponent.Animator.SetBool("Attacking",true);
            }
            else
            {
                animationComponent.Animator.SetBool("Attacking",false);
            }

            if (healthComponent.IsLive == false)
            {
                animationComponent.Animator.SetBool("Death", true);
            }
            else
            {
                animationComponent.Animator.SetBool("Death", false);
            }

            if (movementComponent.Direct != Vector3.zero)
            {
                animationComponent.Animator.SetBool("Moving", true);
            }
            else
            {
                animationComponent.Animator.SetBool("Moving", false);
            }
        }
    }
}