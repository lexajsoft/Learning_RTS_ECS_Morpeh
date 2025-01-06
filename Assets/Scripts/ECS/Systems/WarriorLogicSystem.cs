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
    public sealed class WarriorLogicSystem : UpdateSystem
    {
        private Filter _filter;

        public override void OnAwake()
        {
            World = WorldManager.WorldDefault;
            _filter = World.Filter
                .With<HeroClassComponent>()
                .With<NearEntitiesComponent>()
                .With<MovementComponent>()
                .With<MelleAttackComponent>()
                .With<TagTeamComponent>()
                .With<PositionComponent>()
                .With<HealthComponent>()
                .With<TargetComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var heroClassComponent = ref entity.GetComponent<HeroClassComponent>();
                if (heroClassComponent.HeroClass != HeroClass.Warrior)
                    continue;

                
                ref var healthComponent = ref entity.GetComponent<HealthComponent>();
                if (IsDead(healthComponent))
                {
                    StopMoving();
                    continue;
                }

                ref var melleAttackComponent = ref entity.GetComponent<MelleAttackComponent>();
                ref var positionComponent = ref entity.GetComponent<PositionComponent>();
                ref var targetComponent = ref entity.GetComponent<TargetComponent>();

                // Проверка противника, если противника нет, значит создается запрос на его поиск
                // если противник умер то удаляем об его упоминании
                if (CheckValidTarget(entity, targetComponent) == false)
                {
                    StopMoving();
                    
                    // Создаем запрос на получение ближайших противников поблизости
                    UpdateNearRequestFactory.CreateRequest(entity);
                    FindNearTargetRequestFactory.CreateRequest(entity, TargetType.Enemy);
                    continue;
                }

                var target = targetComponent.Target;
                //if (target.IsDisposed() || !target.Has<PositionComponent>()) continue;
                
                ref var enemyPositionComponent = ref targetComponent.Target.GetComponent<PositionComponent>();

                // перемещение к цели
                var direct = (enemyPositionComponent.Pos - positionComponent.Pos).normalized;
                var distance = Vector3.Distance(enemyPositionComponent.Pos, positionComponent.Pos);

                // если атака уже идет то просто ее проверяем и обновляем, либо дропаем если расстояние уменьшилось
                if (IsCanAttackProcessing(entity))
                {
                    if (TryAttackProcessing(deltaTime, distance, melleAttackComponent, entity, targetComponent))
                    {
                        StopMoving();
                    }
                    else
                    {
                        MoveToDirect(entity, direct);
                    }
                }
                else
                {
                    if (TryStartAttackProcessing(distance, melleAttackComponent, entity, targetComponent) == false)
                    {
                        MoveToDirect(entity, direct);
                    }
                }
                DebugDrawLines(entity, positionComponent, targetComponent);
            }
        }

        private static bool IsDead(HealthComponent healthComponent)
        {
            return healthComponent.IsLive == false;
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

        private static void MoveToDirect(Entity entity, Vector3 direct)
        {
            ref var movementComponent = ref entity.GetComponent<MovementComponent>();
            movementComponent.Direct = direct;
        }

        private static void StopMoving()
        {
            MovementComponent movementComponent;
            movementComponent.Direct = Vector3.zero;
        }

        private static bool IsCanAttackProcessing(Entity entity)
        {
            return entity.Has<AttackProcessingComponent>();
        }

        private static bool TryStartAttackProcessing(float distance, MelleAttackComponent melleAttackComponent, Entity entity,
            TargetComponent targetComponent)
        {
            // если слишком долеко, то процесс атаки не начинаем
            if (!(distance <= melleAttackComponent.DistanceAttack)) return false;
            
            // возможно стоит ее перенести из обычного мира в мир события
            entity.SetComponent(new AttackProcessingComponent()
            {
                Target = targetComponent.Target,
                Timer = new Timer()
                {
                    Remain = melleAttackComponent.AttackTime,
                    Delayed = melleAttackComponent.AttackTime
                },
            });
            return true;

        }

        // проверяем дистанцию
        // если ок атакуем
        // если нет то дропаем атаку
        private bool TryAttackProcessing(float deltaTime, float distance, MelleAttackComponent melleAttackComponent, Entity entity,
            TargetComponent targetComponent)
        {
            
            if (distance <= melleAttackComponent.DistanceAttack)
            {
                // если процесс атаки уже есть то обновляем его и потом наносим урон
                ref var attackProcessingComponent = ref entity.GetComponent<AttackProcessingComponent>();
                if (UpdateTimerAttack(ref attackProcessingComponent, deltaTime))
                {
                    DamageFactory.CreateDamageRequest(targetComponent.Target,
                        melleAttackComponent.GetDamage(), AttackType.SwordBite);
                    entity.RemoveComponent<AttackProcessingComponent>();
                }

                return true;
            }
            else
            {
                if (entity.Has<AttackProcessingComponent>())
                {
                    entity.RemoveComponent<AttackProcessingComponent>();
                }

                return false;
            }
        }

        private bool CheckValidTarget(Entity selfEntity, TargetComponent targetComponent)
        {
            if (targetComponent.Target == null ||
                (targetComponent.Target != null && targetComponent.Target.IsDisposed()))
            {
                if (selfEntity.Has<AttackProcessingComponent>())
                {
                    selfEntity.RemoveComponent<AttackProcessingComponent>();
                }

                targetComponent.Target = null;
                return false;
            }

            if (!targetComponent.Target.Has<HealthComponent>()) return false;

            ref var enemyHealthComponent = ref targetComponent.Target.GetComponent<HealthComponent>();
            if (enemyHealthComponent.IsLive)
                return true;

            targetComponent.Target = null;
            return false;
        }

        private bool UpdateTimerAttack(ref AttackProcessingComponent attackProcessingComponent, float deltaTime)
        {
            return attackProcessingComponent.Timer.UpdateAndCheck(deltaTime);
        }
    }
}