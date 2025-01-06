using System.Collections.Generic;
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

    private string ANIMATION_STATE_ATTACK => "Attacking";
    private string ANIMATION_STATE_DEATH => "Death";
    private string ANIMATION_STATE_MOVING => "Moving";

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            ref var animationComponent = ref entity.GetComponent<AnimationComponent>();
            ref var healthComponent = ref entity.GetComponent<HealthComponent>();
            ref var movementComponent = ref entity.GetComponent<MovementComponent>();

            // разовая инициализация
            // не актуальна после апгрейда
            // if (animationComponent.Inited == false)
            // {
            //     animationComponent.States = new Dictionary<string, int>();
            //     foreach (var parameter in animationComponent.Animator.parameters)
            //     {
            //         animationComponent.States.Add(parameter.name, parameter.nameHash);
            //     }
            // }

            var isAttacking = entity.Has<AttackProcessingComponent>();
            if (animationComponent.States.TryGetValue(ANIMATION_STATE_ATTACK, out var animAttackID))
            {
                animationComponent.Animator.SetBool(animAttackID, isAttacking);
            }

            if (animationComponent.States.TryGetValue(ANIMATION_STATE_DEATH, out var animDeathID))
            {
                animationComponent.Animator.SetBool(animDeathID, !healthComponent.IsLive);
            }

            if (animationComponent.States.TryGetValue(ANIMATION_STATE_MOVING, out var animMoveID))
            {
                animationComponent.Animator.SetBool(animMoveID, movementComponent.Direct != Vector3.zero);
            }
        }
    }
}