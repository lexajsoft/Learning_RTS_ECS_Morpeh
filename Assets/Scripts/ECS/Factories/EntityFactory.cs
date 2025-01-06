using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using ECS.Components;
using ECS.Components.Interface;
using ECS.ScriptableObjects;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

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

            // улучшение, теперь инициализация перенесена в сами компоненты
            for (int i = 0; i < entityComponentsData.Components.Count; i++)
            {
                var component = entityComponentsData.Components[i].Copy();

                if (component is IComponentGameObjectInitting IComponentGameObjectInitting)
                {
                    IComponentGameObjectInitting.Init(gameObject);
                }

                if (component is IComponentEmptyInitting IComponentEmptyInitting)
                {
                    IComponentEmptyInitting.Init();
                }

                AddComponent(entity, component);
            }

            // если на GameObject есть готовый пресет то он дополнительно сверху устанавливается
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
            AddEntityMono(entity, gameObject);
            CreateHealthBar(entity, gameObject);

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
                var component = entityComponentsData.Components[i].Copy();

                if (component is IComponentGameObjectInitting IComponentGameObjectInitting)
                {
                    IComponentGameObjectInitting.Init(gameObject);
                }

                if (component is IComponentEmptyInitting IComponentEmptyInitting)
                {
                    IComponentEmptyInitting.Init();
                }

                AddComponent(entity, component);
            }

            AddEntityMono(entity, gameObject);
            CreateHealthBar(entity, gameObject);
        }

        /// <summary>
        /// Добавляет на объект EntityMono для взаимодействия с Entity
        /// Пример. в NPC попадает стрела которая должена нанести урон, и для этого у NPC запрашивается EntityMono покоторому могут обратиться к Entity 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="gameObject"></param>
        private static void AddEntityMono(Entity entity, GameObject gameObject)
        {
            EntityMono entityMono = gameObject.GetComponent<EntityMono>();
            if (entityMono == null)
                entityMono = gameObject.AddComponent<EntityMono>();
            entityMono.SetEntity(entity);
        }

        private static void CreateHealthBar(Entity entity, GameObject gameObject)
        {
            if (entity.Has<HealthBarComponent>())
            {
                HealthBarFactory.Create(entity, gameObject);
            }
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

        private static IComponent Copy(this IComponent obj)
        {
            IComponent comp = null;
            if (obj is ICloneable ICloneable)
            {
                comp = (IComponent) ICloneable.Clone();
            }
            else
            {
                comp = obj;
            }

            return comp;
        }

        private static void AddComponent(Entity entity, IComponent component)
        {
            // обновленный способ
            Type entityType = typeof(EntityExtensions);
            MethodInfo methodInfo = entityType.GetMethod("SetComponent");

            // Указываем тип параметра
            //Type componentType = component.GetType();
            Type componentType = component.GetType();
            MethodInfo genericMethod = methodInfo.MakeGenericMethod(componentType);

            // Вызываем метод
            genericMethod.Invoke(null, new object[] {entity, component});
        }
    }
}