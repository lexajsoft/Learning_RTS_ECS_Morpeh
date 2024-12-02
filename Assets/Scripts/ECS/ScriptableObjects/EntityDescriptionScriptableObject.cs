using System;
using System.Collections.Generic;
using ECS.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.ScriptableObjects
{

    /// <summary>
    /// Описание юнита
    /// </summary>
    [Serializable]
    public struct EntityDescriptionData
    {
        public string Name;
        public GameObject Prefab;
        [Header("Время создания")]
        public float CreationTime;
    }

    /// <summary>
    /// Базовые компоненты для юнита
    /// </summary>
    [Serializable]
    public struct EntityComponentsData
    {
        [Header("Компоненты")]
        [SerializeReference]
        public List<IComponent> Components;
    }

    [CreateAssetMenu(menuName = "Create/Entity/Unit", fileName = "Unit", order = 0)]
    public class EntityDescriptionScriptableObject : ScriptableObject
    {
        [SerializeField] private EntityDescriptionData _entityDescriptionData;

        [SerializeField] private EntityComponentsData _entityComponentsData = new EntityComponentsData()
        {
            Components = new List<IComponent>
            {
                new PositionComponent()
                {
                    Pos = Vector3.zero
                },
                new TransformComponent(),
                new TagTeamComponent()
            }
        };

        public EntityDescriptionData GetEntityDescriptionData()
        {
            return _entityDescriptionData;
        }
        
        public EntityComponentsData GetEntityComponentsData()
        {
            return _entityComponentsData;
        }
    }
}