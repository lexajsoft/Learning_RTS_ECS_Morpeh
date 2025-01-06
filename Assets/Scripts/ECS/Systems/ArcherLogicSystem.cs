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
                if (IsNotArcherClass(heroClassComponent)) continue;
                if (IsDead(entity))
                {
                    StopMoving(entity);
                    continue;
                }

                ref var attackComponent = ref entity.GetComponent<RangeAttackComponent>();
                ref var positionComponent = ref entity.GetComponent<PositionComponent>();
                ref var targetComponent = ref entity.GetComponent<TargetComponent>();
                
                // проверка цели для атаки
                // если цель не валидна значит надо искать новую
                if (CheckTargetForAttack(targetComponent, entity) == false)
                {
                    UpdateNearRequestFactory.CreateRequest(entity);
                    FindNearTargetRequestFactory.CreateRequest(entity, TargetType.Enemy);
                    continue;
                }

                var target = targetComponent.Target;
                if (!target.Has<PositionComponent>()) continue;
                
                ref PositionComponent enemyPositionComponent = ref targetComponent.Target.GetComponent<PositionComponent>();
                // направление к цели
                Vector3 direct = (enemyPositionComponent.Pos - positionComponent.Pos).normalized;
                // расстояние до цели
                float distance = Vector3.Distance(enemyPositionComponent.Pos, positionComponent.Pos);


                // если атака уже идет то просто ее проверяем и обновляем, либо дропаем если расстояние уменьшилось
                if (IsCanAttackProcessing(entity))
                {
                    // Если удается дамажить, дамажим и стоим
                    // если не удается дамажить, то бежим к цели
                    if (TryAttackProcessing(deltaTime, distance, entity, direct))
                    {
                        StopMoving(entity);
                    }
                    else
                    {
                        MoveToDirect(entity, direct);
                    }
                }
                else
                {
                    if (TryStartAttackProcessing(distance, attackComponent, entity, targetComponent) == false)
                    {
                        MoveToDirect(entity,direct);
                    }
                }

                DebugDrawLines(entity, positionComponent, targetComponent);
            }
        }

        private static void MoveToDirect(Entity entity, Vector3 direct)
        {
            if (entity.Has<MovementComponent>())
            {
                ref var movementComponent = ref entity.GetComponent<MovementComponent>();
                movementComponent.Direct = direct;
            }
        }

        private static void StopMoving(Entity entity)
        {
            if (entity.Has<MovementComponent>())
            {
                ref var movementComponent = ref entity.GetComponent<MovementComponent>();
                movementComponent.Direct = Vector3.zero;
            }
        }

        private static bool IsNotArcherClass(HeroClassComponent heroClassComponent)
        {
            if (heroClassComponent.HeroClass != HeroClass.Archer)
                return true;
            return false;
        }

        private static bool IsDead(Entity entity)
        {
            ref var healthComponent = ref entity.GetComponent<HealthComponent>();
            return healthComponent.IsLive == false;
        }

        private static bool TryStartAttackProcessing(float distance, RangeAttackComponent attackComponent, Entity entity,
            TargetComponent targetComponent)
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
                });
                return true;
            }

            return false;
        }

        private static bool IsCanAttackProcessing(Entity entity)
        {
            return entity.Has<AttackProcessingComponent>();
        }

        private bool TryAttackProcessing(float deltaTime, float distance, Entity entity, Vector3 direct)
        {
            ref var tagComponent = ref entity.GetComponent<TagTeamComponent>();
            ref var attackComponent = ref entity.GetComponent<RangeAttackComponent>();
            ref var quiverComponent = ref entity.GetComponent<QuiverComponent>();
            ref var positionComponent = ref entity.GetComponent<PositionComponent>();
            
            if (distance <= attackComponent.DistanceAttack)
            {
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
                return true;
            }

            if (entity.Has<AttackProcessingComponent>())
            {
                entity.RemoveComponent<AttackProcessingComponent>();
            }

            return false;
        }

        private static bool CheckTargetForAttack(TargetComponent targetComponent, Entity entity)
        {
            if (IsValidTarget(targetComponent) == false)
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
                
                return false;
            }

            if (targetComponent.Target.Has<HealthComponent>())
            {
                ref var enemyHealthComponent = ref targetComponent.Target.GetComponent<HealthComponent>();
                if (enemyHealthComponent.IsLive == false)
                {
                    targetComponent.Target = null;
                    return false;
                }
            }
            return true;
        }

        private static bool IsValidTarget(TargetComponent targetComponent)
        {
            return targetComponent.Target != null && targetComponent.Target.IsDisposed() == false;
        }

        private static void DebugDrawLines(Entity entity, PositionComponent positionComponent, TargetComponent targetComponent)
        {
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