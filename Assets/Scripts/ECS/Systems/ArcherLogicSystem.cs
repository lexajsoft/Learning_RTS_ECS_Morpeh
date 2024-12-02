using ECS.Components;
using ECS.Factories;
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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WarriorLogicSystem))]
    public sealed class ArcherLogicSystem : UpdateSystem
    {
        private Filter _filter;
    
        public override void OnAwake()
        {
            World = WorldManager.WorldDefault;
            _filter = World.Filter
                .With<HeroClassComponent>()
                .With<RangeAttackComponent>()
                .With<TagTeamComponent>()
                .With<PositionComponent>()
                .With<HealthComponent>()
                .With<TargetComponent>()
                .With<QuiverComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in _filter)
            {
                ref var heroClassComponent = ref entity.GetComponent<HeroClassComponent>();
                if(heroClassComponent.HeroClass != HeroClass.Archer)
                    continue;
                
                ref var healthComponent = ref entity.GetComponent<HealthComponent>();
                if (healthComponent.IsLive == false)
                {
                    if (entity.Has<MovementComponent>())
                    {
                        ref var movementComponent = ref entity.GetComponent<MovementComponent>();
                        movementComponent.Direct = Vector3.zero;
                    }

                    continue;
                }
                
                ref var tagComponent = ref entity.GetComponent<TagTeamComponent>();
                ref var attackComponent = ref entity.GetComponent<RangeAttackComponent>();
                ref var quiverComponent = ref entity.GetComponent<QuiverComponent>();
                ref var positionComponent = ref entity.GetComponent<PositionComponent>();
                ref var targetComponent = ref entity.GetComponent<TargetComponent>();
                
                // если цели нет, то удаляем упоминание о ней
                // создаем запрос на обновление ближайших противников и создаем запрос на поиск противника
                //if (targetComponent.Target == null || (targetComponent.Target != null && targetComponent.Target.IsDisposed()))
                if (targetComponent.Target == null || (targetComponent.Target != null && targetComponent.Target.IsDisposed()))
                {
                    if (entity.Has<AttackProcessingComponent>())
                    {
                        entity.RemoveComponent<AttackProcessingComponent>();
                    }

                    targetComponent.Target = null;
                   
                    if (entity.Has<MovementComponent>())
                    {
                        ref var movementComponent = ref entity.GetComponent<MovementComponent>();
                        movementComponent.Direct = Vector3.zero;
                    }
                   
                    
                    UpdateNearRequestFactory.CreateRequest(entity);
                    FindNearTargetRequestFactory.CreateRequest(entity, TargetType.Enemy);
                    continue;
                }
                else
                {
                    if (targetComponent.Target.Has<HealthComponent>())
                    {
                        ref var enemyHealthComponent = ref targetComponent.Target.GetComponent<HealthComponent>();
                        if (enemyHealthComponent.IsLive == false)
                        {
                            targetComponent.Target = null;
                            continue;
                        }
                        
                    }
                }

                var target = targetComponent.Target;
                
                if (target.IsDisposed() == false && target.Has<PositionComponent>())
                {
                    ref PositionComponent enemyPositionComponent =
                        ref targetComponent.Target.GetComponent<PositionComponent>();

                    // перемещение к цели
                    Vector3 direct = (enemyPositionComponent.Pos - positionComponent.Pos).normalized;
                    float distance = Vector3.Distance(enemyPositionComponent.Pos, positionComponent.Pos);

                    // если атака уже идет то просто ее проверяем и обновляем, либо дропаем если расстояние уменьшилось
                    if (entity.Has<AttackProcessingComponent>())
                    {
                        // проверяем дистанцию
                        // если ок атакуем
                        // если нет то дропаем атаку

                        if (distance <= attackComponent.DistanceAttack)
                        {
                            if (entity.Has<MovementComponent>())
                            {
                                ref var movementComponent = ref entity.GetComponent<MovementComponent>();
                                movementComponent.Direct = Vector3.zero;
                            }
                            
                            // если процесс атаки уже есть то обновляем его и потом наносим урон
                            ref var attackProcessingComponent = ref entity.GetComponent<AttackProcessingComponent>();
                            var offset = new Vector3(0, 0.5f, 0);
                            if (UpdateTimerAttack(ref attackProcessingComponent, deltaTime))
                            {
                                var projectileData = quiverComponent.ProjectileData;
                                projectileData.Direct = direct;
                                projectileData.StartPosition = positionComponent.Pos + offset;
                                ProjectileFactory.CreateProjectile(projectileData, tagComponent.TagTeam);
                                entity.RemoveComponent<AttackProcessingComponent>();
                            }
                        }
                        else
                        {
                            if (entity.Has<MovementComponent>())
                            {
                                ref var movementComponent = ref entity.GetComponent<MovementComponent>();
                                movementComponent.Direct = direct;
                            }
                           
                            if (entity.Has<AttackProcessingComponent>())
                            {
                                entity.RemoveComponent<AttackProcessingComponent>();
                            }
                        }
                    }
                    else
                    {
                        if (distance <= attackComponent.DistanceAttack)
                        {
                            // возможно стоит ее перенести из обычного мира в мир события
                            entity.SetComponent(new AttackProcessingComponent()
                            {
                                Target = targetComponent.Target,
                                Timer = new Timer()
                                {
                                    Remain = attackComponent.AttackTime,
                                    Delayed = attackComponent.AttackTime
                                },
                                //Damage = attackComponent.GetDamage()
                            });
                        }
                        else
                        {
                            if (entity.Has<MovementComponent>())
                            {
                                ref var movementComponent = ref entity.GetComponent<MovementComponent>();
                                movementComponent.Direct = direct;
                            }
                        }
                    }

#if UNITY_EDITOR
                    // DEBUG
                    // рисует путь до цели атаки
                    if (entity.Has<AttackProcessingComponent>())
                    {
                        ref var attackProcessingComponent = ref entity.GetComponent<AttackProcessingComponent>();

                        if (attackProcessingComponent.Target != null &&
                            attackProcessingComponent.Target.IsDisposed() == false)
                        {
                            ref var pos = ref attackProcessingComponent.Target.GetComponent<PositionComponent>();
                            Debug.DrawLine(positionComponent.Pos, pos.Pos, Color.red);
                        }
                    }
                    
                    // рисует путь до цели
                    if (targetComponent.Target != null && targetComponent.Target.IsDisposed() == false)
                    {
                        ref var posTarget = ref targetComponent.Target.GetComponent<PositionComponent>();
                        Debug.DrawLine(positionComponent.Pos, posTarget.Pos, Color.blue);
                    }
#endif
                }
            }
        }

        private bool UpdateTimerAttack(ref AttackProcessingComponent attackProcessingComponent, float deltaTime)
        {
            if (attackProcessingComponent.Timer.UpdateAndCheck(deltaTime))
            {
                return true;
            }
            return false;
        }
    }
}