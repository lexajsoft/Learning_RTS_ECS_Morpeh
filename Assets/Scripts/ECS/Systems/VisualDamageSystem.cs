using ECS.Components;
using ECS.Factories;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(VisualDamageSystem))]
public sealed class VisualDamageSystem : UpdateSystem
{

    private Filter _filter;
    
    public override void OnAwake()
    {
        _filter = World.Filter
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
                if (damageRequestComponent.Target.Has<PositionComponent>())
                {
                    ref var positionComponent = ref damageRequestComponent.Target.GetComponent<PositionComponent>();

                    VisualEffectFactory.CreateVisual(positionComponent.Pos, damageRequestComponent.AttackType);
                }
            }

            
        }
    }
}