using System;
using System.Collections.Generic;
using System.Reflection;
using ECS.Components;
using ECS.ScriptableObjects;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ECS.Factories
{
    public static class EntityFactory
    {
        /// <summary>
        /// Создает энтити и игровой объект
        /// </summary>
        /// <param name="entityDescriptionData"></param>
        /// <param name="entityComponentsData"></param>
        /// <param name="container"></param>
        /// <param name="tagTeam"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Entity CreateEntity(
            EntityDescriptionData entityDescriptionData,
            EntityComponentsData entityComponentsData,
            Transform container,
            TagTeam tagTeam,
            Vector3 position,
            Quaternion quaternion)
        {
            var gameObject = GameObject.Instantiate(entityDescriptionData.Prefab, container);
            gameObject.transform.position = position;
            gameObject.transform.rotation = quaternion;

#if UNITY_EDITOR
            var entityProvider = gameObject.AddComponent<EntityProvider>();
            var entity = entityProvider.Entity;
#else
            var entity = WorldManager.WorldDefault.CreateEntity();
#endif

            for (int i = 0; i < entityComponentsData.Components.Count; i++)
            {
                AddComponent(entity, entityComponentsData.Components[i]);
            }

            var presetEntityMono = gameObject.GetComponentInChildren<PresetEntityMono>();
            if (presetEntityMono != null)
            {
                var data = presetEntityMono.GetEntityComponentsData();
                for (int i = 0; i < data.Components.Count; i++)
                {
                    AddComponent(entity, data.Components[i]);
                }
            }

            SetTag(entity, gameObject, tagTeam);
            SetValues(entity, gameObject);

            return entity;
        }

        /// <summary>
        /// Создает энитити для объекта который уже создан и для него надо сделать энтити
        /// </summary>
        /// <param name="entityComponentsData"></param>
        /// <param name="gameObject"></param>
        public static void UpdateEntity(EntityComponentsData entityComponentsData, GameObject gameObject)
        {
#if UNITY_EDITOR
            var entityProvider = gameObject.AddComponent<EntityProvider>();
            var entity = entityProvider.Entity;
#else
            var entity = WorldManager.WorldDefault.CreateEntity();
#endif


            for (int i = 0; i < entityComponentsData.Components.Count; i++)
            {
                AddComponent(entity, entityComponentsData.Components[i]);
            }

            if (entity.Has<TagTeamComponent>())
            {
                ref var tagTeamComponent = ref entity.GetComponent<TagTeamComponent>();

                if (gameObject.TryGetComponent<ReColoringByTagTeam>(out var reColoringByTagTeam))
                {
                    reColoringByTagTeam.ReColor(tagTeamComponent.TagTeam);
                }
            }

            SetValues(entity, gameObject);
        }

        /// <summary>
        /// Прокидывание в компоненты данные
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="gameObject"></param>
        private static void SetValues(Entity entity, GameObject gameObject)
        {
            if (entity.Has<TransformComponent>())
            {
                ref var pos = ref entity.GetComponent<TransformComponent>();
                pos.Transform = gameObject.transform;
            }

            if (entity.Has<PositionComponent>())
            {
                ref var pos = ref entity.GetComponent<PositionComponent>();
                pos.Pos = gameObject.transform.position;
            }

            if (entity.Has<AnimationComponent>())
            {
                ref var animationComponent = ref entity.GetComponent<AnimationComponent>();

                animationComponent.Animator = gameObject.GetComponent<Animator>();
            }

            if (entity.Has<MovementComponent>())
            {
                ref var movementComponent = ref entity.GetComponent<MovementComponent>();
                if (gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
                {
                    movementComponent.Transform = gameObject.transform;
                    movementComponent.Rigidbody = rigidbody;
                }
            }

            if (entity.Has<TargetComponent>())
            {
                ref var targetComponent = ref entity.GetComponent<TargetComponent>();
                targetComponent.Target = null;
            }

            if (entity.Has<HealthBarComponent>())
            {
                HealthBarFactory.Create(entity, gameObject);
            }

            if (entity.Has<DeactivateColliderOnDeadComponent>())
            {
                ref var deactivateColliderOnDeadComponent =
                    ref entity.GetComponent<DeactivateColliderOnDeadComponent>();
                deactivateColliderOnDeadComponent.Collider = gameObject.GetComponent<Collider>();
                if (gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
                {
                    deactivateColliderOnDeadComponent.Rigidbody = rigidbody;
                }
            }

            if (entity.Has<SpawnComponent>())
            {
                ref var spawnComponent = ref entity.GetComponent<SpawnComponent>();
                spawnComponent.QueueEntity = new List<EntityDescriptionScriptableObject>();
            }

            EntityMono entityMono = gameObject.GetComponent<EntityMono>();
            if (entityMono == null)
                entityMono = gameObject.AddComponent<EntityMono>();
            entityMono.SetEntity(entity);
        }

        private static void SetTag(Entity entity, GameObject gameObject, TagTeam tagTeam)
        {
            if (entity.Has<TagTeamComponent>())
            {
                ref var tagTeamComponent = ref entity.GetComponent<TagTeamComponent>();
                tagTeamComponent.TagTeam = tagTeam;

                if (gameObject.TryGetComponent<ReColoringByTagTeam>(out var reColoringByTagTeam))
                {
                    reColoringByTagTeam.ReColor(tagTeam);
                }
            }
        }

        private static void AddComponent(Entity entity, IComponent component)
        {
            // обновленный способ
            Type entityType = typeof(EntityExtensions);
            MethodInfo methodInfo = entityType.GetMethod("SetComponent");

            // Указываем тип параметра
            Type componentType = component.GetType();
            MethodInfo genericMethod = methodInfo.MakeGenericMethod(componentType);

            // Вызываем метод
            genericMethod.Invoke(null, new object[] {entity, component});

            ///////////////////////////////////////////////////////////////////////////

            // Ранее я юзал подключение компонентов именно вот так

            // class 
            // if(entity.CheckAndSet<HeroClassComponent>(component))return;
            //
            // // health
            // if(entity.CheckAndSet<HealthComponent>(component))return;
            //
            // // movement
            // if(entity.CheckAndSet<PositionComponent>(component))return;
            // if(entity.CheckAndSet<MovementComponent>(component))return;
            // if(entity.CheckAndSet<RotationComponent>(component))return;
            //
            // // animations
            // if(entity.CheckAndSet<AnimationComponent>(component))return;
            //
            // // actions
            // if(entity.CheckAndSet<MelleAttackComponent>(component))return;
            // if(entity.CheckAndSet<RangeAttackComponent>(component))return;
            // if(entity.CheckAndSet<QuiverComponent>(component)) return;
            //
            // // targets
            // if(entity.CheckAndSet<MainTargetComponent>(component))return;
            // if(entity.CheckAndSet<NearEntitiesComponent>(component))return;
            // if(entity.CheckAndSet<TargetComponent>(component)) return;
            //
            // // TagTeam
            // if(entity.CheckAndSet<TagTeamComponent>(component)) return;
            //
            // // Transform
            // if(entity.CheckAndSet<TransformComponent>(component)) return;
            //
            // // Spawn logic
            // if(entity.CheckAndSet<SpawnPointsComponent>(component)) return;
            // if(entity.CheckAndSet<SpawnComponent>(component)) return;
            // if(entity.CheckAndSet<EntitySpawnProccesingComponent>(component)) return;
            // if(entity.CheckAndSet<EntitySpawnReadyComponent>(component)) return;
        }

        // old
        private static bool CheckAndSet<T>(this Entity entity, IComponent component) where T : struct, IComponent
        {
            if (component is T result)
            {
                entity.SetComponent(result);
                return true;
            }

            return false;
        }
    }
}