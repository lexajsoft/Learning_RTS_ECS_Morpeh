using ECS.Components;
using ECS.Factories;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ProjectileTriggerEventSystem))]
    public sealed class ProjectileTriggerEventSystem : UpdateSystem
    {
        private Filter _filter;

        public override void OnAwake()
        {
            _filter = World.Filter
                .With<ProjectileTriggerEventRequestComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var component = ref entity.GetComponent<ProjectileTriggerEventRequestComponent>();

                if (component.Entity == null || component.OtherEntity == null)
                {
                    entity.Dispose();
                    continue;
                }

                if ((component.Entity != null && component.Entity.IsDisposed()) || (component.OtherEntity != null &&
                        component.OtherEntity.IsDisposed()))
                {
                    entity.Dispose();
                    continue;
                }

                if (component.Entity.Has<ProjectileComponent>() == false)
                {
                    entity.Dispose();
                    continue;
                }

                if (component.Entity.Has<TagTeamComponent>() == false)
                {
                    entity.Dispose();
                    continue;
                }

                if (component.OtherEntity.Has<TagTeamComponent>() == false)
                {
                    entity.Dispose();
                    continue;
                }

                ref var tagTeamComponent = ref component.Entity.GetComponent<TagTeamComponent>();
                ref var otherTagTeamComponent = ref component.OtherEntity.GetComponent<TagTeamComponent>();
                if (tagTeamComponent.TagTeam == otherTagTeamComponent.TagTeam)
                {
                    entity.Dispose();
                    continue;
                }

                if (component.OtherEntity.Has<HealthComponent>() == false)
                {
                    entity.Dispose();
                    continue;
                }

                ref var healthComponent = ref component.OtherEntity.GetComponent<HealthComponent>();
                if (healthComponent.IsLive == false)
                {
                    entity.Dispose();
                    continue;
                }

                ref var projectileComponent = ref component.Entity.GetComponent<ProjectileComponent>();
                DamageFactory.CreateDamageRequest(component.OtherEntity, projectileComponent.Damage,
                    AttackType.ArrowBite);

                // удаление стрелы
                component.Entity.Dispose();
                Destroy(component.GameObject);

                entity.Dispose();
            }
        }
    }
}