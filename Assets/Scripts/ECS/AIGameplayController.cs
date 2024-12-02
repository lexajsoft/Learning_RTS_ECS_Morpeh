using System.Collections.Generic;
using ECS.Components;
using ECS.ScriptableObjects;
using Extension;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS
{
    public class AIGameplayController : GameplayControllerBase
    {
        [SerializeField] private List<EntityDescriptionScriptableObject> _units;
        [SerializeField] private float _periodCreateUnit = 2f;
        private float _timeRemainForCreateRequest;

        public override void Init()
        {
            _units = new List<EntityDescriptionScriptableObject>();

            if (_entity.Has<SpawnComponent>())
            {
                ref var spawnComponent = ref _entity.GetComponent<SpawnComponent>();
                for (int i = 0; i < spawnComponent.PoolEntitys.Count; i++)
                {
                    _units.Add(spawnComponent.PoolEntitys[i]);
                }
            }

            _timeRemainForCreateRequest = _periodCreateUnit;
        }

        private void Update()
        {
            if (_entity.IsNullOrDisposed())
            {
                Debug.Log("Бот уничтожен");
                Destroy(gameObject);
                return;
            }

            if (_timeRemainForCreateRequest > 0)
            {
                _timeRemainForCreateRequest -= Time.deltaTime;
            }
            else
            {
                SendRequestToCreateRandomUnit();
                _timeRemainForCreateRequest = _periodCreateUnit;
            }
        }

        private void SendRequestToCreateRandomUnit()
        {
            var data = _units.GetRandom();
            if (_entity.Has<SpawnComponent>() && _entity.Has<HealthComponent>())
            {
                ref var healthComponent = ref _entity.GetComponent<HealthComponent>();
                if (healthComponent.IsLive == false)
                    return;

                ref var spawnComponent = ref _entity.GetComponent<SpawnComponent>();
                spawnComponent.QueueEntity.Add(ScriptableObject.Instantiate(data));

                _timeRemainForCreateRequest = data.GetEntityDescriptionData().CreationTime + 1;
            }
        }
    }
}