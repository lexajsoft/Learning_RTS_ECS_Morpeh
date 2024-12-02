using System;
using System.Collections.Generic;
using System.Linq;
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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(NearEntitySystem))]
    public sealed class NearEntitySystem : UpdateSystem
    {
        private Filter _allUnitfilter;
        private Filter _requestFilter;

        public override void OnAwake()
        {
            _allUnitfilter = WorldManager.WorldDefault.Filter
                .With<HealthComponent>()
                .With<PositionComponent>()
                .With<TagTeamComponent>()
                .Build();


            _requestFilter = WorldManager.WorldEvent.Filter
                .With<UpdateNearEntityRequestComponent>()
                .Build();

        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityRequest in _requestFilter)
            {
                ref var updateNearEntityRequestComponent =
                    ref entityRequest.GetComponent<UpdateNearEntityRequestComponent>();


                if (updateNearEntityRequestComponent.Entity.Has<NearEntitiesComponent>())
                {
                    ref NearEntitiesComponent nearEntitiesComponent =
                        ref updateNearEntityRequestComponent.Entity.GetComponent<NearEntitiesComponent>();

                    ref var mainPositionComponent =
                        ref updateNearEntityRequestComponent.Entity.GetComponent<PositionComponent>();

                    // ref var selfPositionComponent = ref updateNearEntityRequestComponent.Entity.GetComponent<PositionComponent>();
                    ref var selfTagTeamComponent =
                        ref updateNearEntityRequestComponent.Entity.GetComponent<TagTeamComponent>();

                    nearEntitiesComponent.Allies = new List<Entity>(10);
                    nearEntitiesComponent.Enemies = new List<Entity>(10);

                    foreach (var otherEntity in _allUnitfilter)
                    {
                        ref var otherTagTeamComponent = ref otherEntity.GetComponent<TagTeamComponent>();

                        // оставить на тот случай если захочу вернуть 
                        //ref var otherEntityPositionComponent = ref otherEntity.GetComponent<PositionComponent>();

                        // пропускаем в случае если нашли самого себя
                        if (otherEntity == updateNearEntityRequestComponent.Entity)
                            continue;

                        // добавление союзника
                        if (otherTagTeamComponent.TagTeam == selfTagTeamComponent.TagTeam)
                        {
                            nearEntitiesComponent.Allies.Add(otherEntity);
                        }

                        // добавление противника
                        if (otherTagTeamComponent.TagTeam != selfTagTeamComponent.TagTeam)
                        {
                            nearEntitiesComponent.Enemies.Add(otherEntity);
                        }
                    }


                    nearEntitiesComponent.Enemies =
                        nearEntitiesComponent.Enemies.OrderBy(KeySelector(mainPositionComponent)).ToList();

                    World.RemoveEntity(entityRequest);
                }
            }
        }

        private static Func<Entity, float> KeySelector(PositionComponent mainPositionComponent)
        {
            return item =>
            {
                ref var posCom = ref item.GetComponent<PositionComponent>();
                return Vector3.Distance(mainPositionComponent.Pos, posCom.Pos);
            };
        }
    }
}