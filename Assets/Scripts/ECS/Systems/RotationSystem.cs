using ECS;
using ECS.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(RotationSystem))]
public sealed class RotationSystem : UpdateSystem
{

    private Filter _filter;
    
    public override void OnAwake()
    {
        World = WorldManager.WorldDefault;
        _filter = World.Filter
            .With<MovementComponent>()
            .With<RotationComponent>()
            .With<PositionComponent>()
            .With<TransformComponent>()
            .Build();

    }

    public override void OnUpdate(float deltaTime) 
    {
        foreach (var entity in _filter)
        {
            // запрашивается так как не все элементы могут иметь хп
            if (entity.Has<HealthComponent>())
            {
                ref var healthComponent = ref entity.GetComponent<HealthComponent>();
                if (healthComponent.IsLive == false)
                    continue;
            }
            

            ref var movementComponent = ref entity.GetComponent<MovementComponent>();
            ref var rotationComponent = ref entity.GetComponent<RotationComponent>();
            ref var transformComponent = ref entity.GetComponent<TransformComponent>();
            ref var positionComponent = ref entity.GetComponent<PositionComponent>();

            // если есть цель, то смотрим в сторону цели
            // если каким то макаром у нас есть компонент с целью, но цели в нем нет то просто смотрим в сторону ходьбы либо никуда не смортим
            if (entity.Has<TargetComponent>())
            {
                ref var targetComponent = ref entity.GetComponent<TargetComponent>();
                if (targetComponent.Target != null && targetComponent.Target.IsDisposed() == false)
                {
                    ref var targetPositionComponent = ref targetComponent.Target.GetComponent<PositionComponent>();
                    Vector3 direct = (targetPositionComponent.Pos - positionComponent.Pos).normalized;  
                    rotationComponent.DirectToLook =  Vector3.Lerp(rotationComponent.DirectToLook, direct,
                        rotationComponent.SpeedRotation * deltaTime);
                }
                else
                {
                    LookToMoveDirect(ref rotationComponent, ref movementComponent, deltaTime);    
                }
            }
            else
            {
                LookToMoveDirect(ref rotationComponent, ref movementComponent, deltaTime);
            }
            
            transformComponent.Transform.LookAt(transformComponent.Transform.position + rotationComponent.DirectToLook);
        }
    }

    private static void LookToMoveDirect(ref RotationComponent rotationComponent, ref MovementComponent movementComponent,
       float deltaTime)
    {
        if (movementComponent.Direct != Vector3.zero)
        {
            rotationComponent.DirectToLook = Vector3.Lerp(rotationComponent.DirectToLook, movementComponent.Direct,
                rotationComponent.SpeedRotation * deltaTime);
        }
    }
}