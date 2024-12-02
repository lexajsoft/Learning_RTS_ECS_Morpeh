using System;
using ECS.Factories;
using ECS.ScriptableObjects;
using TriInspector;
using UnityEngine;

namespace ECS
{
    public class PresetEntityMono : MonoBehaviour
    {
        
        [Header("Создает entity и устанавливает ему компоненты")]
        [SerializeField] private EntityComponentsData _entityDescriptionData;

        
        [SerializeField] private bool _createSelf = false;

        public EntityComponentsData GetEntityComponentsData() => _entityDescriptionData;
        private void Start()
        {
            if(_createSelf)
                EntityFactory.UpdateEntity(_entityDescriptionData, gameObject);
        }
    }
}