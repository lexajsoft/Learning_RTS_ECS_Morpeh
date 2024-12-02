using ECS.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(HealthBarSystem))]
public sealed class HealthBarSystem : UpdateSystem
{
    private Filter _filter;
    public override void OnAwake()
    {
        _filter = World.Filter
            .With<HealthBarComponent>()
            .With<HealthComponent>()
            .Build();
    }

    public override void OnUpdate(float deltaTime) 
    {
        foreach (var entity in _filter)
        {
            ref var healthComponent = ref entity.GetComponent<HealthComponent>();
            ref var healthBarComponent = ref entity.GetComponent<HealthBarComponent>();

            if (healthComponent.IsLive)
            {
                healthBarComponent.ProgressBar.SetValue((float)healthComponent.HealthCurrent / healthComponent.HealthMax);
            }
            else
            {
                healthBarComponent.ProgressBar.DestroySelf();
                entity.RemoveComponent<HealthBarComponent>();
            }
        }
        
        
    }
}