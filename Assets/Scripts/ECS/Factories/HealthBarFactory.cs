using Core;
using ECS.Components;
using ECS.Factories.Data;
using ECS.ScriptableObjects;
using ECS.Untils;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Factories
{
    public static class HealthBarFactory
    {
        private static HealthBarScriptableObject _healthBarScriptableObject;
        
        public static void Init()
        {
            _healthBarScriptableObject = ServiceLocator.Get<HealthBarScriptableObject>();
        }

        public static void Create(Entity entity, GameObject target)
        {
            if (entity.Has<HealthBarComponent>() == false)
            {
                return;
            }

            ref var healthBarComponent = ref entity.GetComponent<HealthBarComponent>();
            
            
            var healthBarGameObject = GameObject.Instantiate(_healthBarScriptableObject.GetPrefab(), target.transform);

            healthBarGameObject.transform.localPosition = healthBarComponent.VisualOffset; 
            // добавление скрипта прогрессбара
            var progressBar = healthBarGameObject.GetComponent<ProgressBar>();
            progressBar.Init();
            
            // привязка хп Бара и установка значения по дефолту
            
            healthBarComponent.ProgressBar = progressBar; 
            progressBar.SetValue(1);
            
        }
    }
}