using ECS.Components;
using Extension;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(FindTargetSystem))]
    // система которая ищет ближаших противников
    public sealed class FindTargetSystem : UpdateSystem
    {
        private Filter _requestFilter;
        public override void OnAwake()
        {
            _requestFilter = World.Filter
                .With<FindNearTargetEntityRequestComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in _requestFilter)
            {
                // запрос
                ref var findNearTargetEntityRequestComponent = ref entity.GetComponent<FindNearTargetEntityRequestComponent>();

                // если энтити каким то макаром пустая то мы ее дропаем
                if (findNearTargetEntityRequestComponent.Entity == null)
                {
                    World.RemoveEntity(entity);
                    continue;
                }

                // если на объекте для которого нужно выполнить запрос без нужных компонентов, то значит опять дропаем
                if (findNearTargetEntityRequestComponent.Entity.Has<NearEntitiesComponent>() == false)
                {
                    World.RemoveEntity(entity);
                    continue;
                }

                if (findNearTargetEntityRequestComponent.Entity.Has<NearEntitiesComponent>() == false)
                {
                    World.RemoveEntity(entity);
                    continue;
                }
                
                
                ref var nearEnemiesComponent = ref findNearTargetEntityRequestComponent.Entity.GetComponent<NearEntitiesComponent>();
                ref var tagTeamComponent = ref findNearTargetEntityRequestComponent.Entity.GetComponent<TagTeamComponent>();
                
                // поиск противника (с условие если противник со здоровьем)
                if (findNearTargetEntityRequestComponent.FindTargetType == TargetType.Enemy)
                {
                    if (nearEnemiesComponent.Enemies.Count > 0)
                    {
                        //var enemy = nearEnemiesComponent.Enemies.GetRandom();
                        for (int i = 0; i < nearEnemiesComponent.Enemies.Count; i++)
                        {
                            var enemy = nearEnemiesComponent.Enemies[i];
                            if (enemy.Has<TagTeamComponent>() && enemy.Has<HealthComponent>())
                            {
                                ref var enemyTagTeamComponent = ref enemy.GetComponent<TagTeamComponent>();
                                ref var healthComponent = ref enemy.GetComponent<HealthComponent>();
                                if (healthComponent.IsLive && enemyTagTeamComponent.TagTeam != tagTeamComponent.TagTeam)
                                {
                                    findNearTargetEntityRequestComponent.Entity.SetComponent(new TargetComponent()
                                        {Target = enemy});
                                    break;
                                }
                            }
                        }
                    }
                }
                
                // поиск союзника
                if (findNearTargetEntityRequestComponent.FindTargetType == TargetType.Allies)
                {
                    if (nearEnemiesComponent.Allies.Count > 0)
                    {
                        // var allie = nearEnemiesComponent.Allies.GetRandom();
                        // if (allie.Has<TagTeamComponent>())
                        // {
                        //     ref var enemyTagTeamComponent = ref allie.GetComponent<TagTeamComponent>();
                        //     if (enemyTagTeamComponent.TagTeam == tagTeamComponent.TagTeam)
                        //     {
                        //         entity.SetComponent(new TargetComponent() {Target = allie});
                        //     }
                        // }
                        
                        for (int i = 0; i < nearEnemiesComponent.Allies.Count; i++)
                        {
                            var allie = nearEnemiesComponent.Allies[i];
                            if (allie.Has<TagTeamComponent>() && allie.Has<HealthComponent>())
                            {
                                ref var enemyTagTeamComponent = ref allie.GetComponent<TagTeamComponent>();
                                ref var healthComponent = ref allie.GetComponent<HealthComponent>();
                                if (healthComponent.IsLive && enemyTagTeamComponent.TagTeam == tagTeamComponent.TagTeam)
                                {
                                    findNearTargetEntityRequestComponent.Entity.SetComponent(new TargetComponent()
                                        {Target = allie});
                                    break;
                                }
                            }
                        }
                    }
                }

                World.RemoveEntity(entity);
            }
            
            
            // поиск цели
            // foreach (var entity in _withoutTargetFilter)
            // {
            //     ref var nearEnemiesComponent = ref entity.GetComponent<NearEntitiesComponent>();
            //     ref var tagTeamComponent = ref entity.GetComponent<TagTeamComponent>();
            //
            //     if (nearEnemiesComponent.Enemies.Count > 0)
            //     {
            //         var enemy = nearEnemiesComponent.Enemies.GetRandom();
            //         if (enemy.Has<TagTeamComponent>())
            //         {
            //             ref var enemyTagTeamComponent = ref enemy.GetComponent<TagTeamComponent>();
            //             if (enemyTagTeamComponent.TagTeam != tagTeamComponent.TagTeam)
            //             {
            //                 entity.SetComponent(new TargetComponent(){Target = enemy});        
            //             }
            //         }
            //     }
            //
            //     // if (entity.Has<TargetComponent>())
            //     // {
            //     //     float distance = 100000;
            //     //     float dist = 0;
            //     //     Entity target = null;
            //     //     
            //     //     for (int i = 0; i < nearComponent.Enemies.Count; i++)
            //     //     {
            //     //         if (nearComponent.Enemies[i].Has<PositionComponent>())
            //     //         {
            //     //             ref var posTarget = ref nearComponent.Enemies[i].GetComponent<PositionComponent>();
            //     //             dist = Vector3.Distance(positionComponent.Pos, posTarget.Pos);
            //     //             if (dist < distance)
            //     //             {
            //     //                 distance = dist;
            //     //                 target = nearComponent.Enemies[i];
            //     //             }
            //     //         }
            //     //     }
            //     //
            //     //     if (target != null)
            //     //     {
            //     //         
            //     //     }
            //     // }
            // }

            // дроп цели если цель мертва
            // foreach (var entity in _checkTargetFilter)
            // {
            //     ref var targetComponent = ref entity.GetComponent<TargetComponent>();
            //
            //     if (targetComponent.Target == null)
            //     {
            //         continue;
            //     }
            //
            //     if (targetComponent.Target.IsDisposed() == false && targetComponent.Target.Has<HealthComponent>())
            //     {
            //         ref var enemyHealt = ref targetComponent.Target.GetComponent<HealthComponent>();
            //         if (enemyHealt.IsLive == false)
            //         {
            //             targetComponent.Target = null;
            //             //entity.RemoveComponent<TargetComponent>();
            //         }
            //     }
            // }
        }
    }
}