using System.Collections.Generic;
using ECS.Components;
using ECS.ScriptableObjects;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS
{
    public class PlayerGameplayController : GameplayControllerBase
    {
        [SerializeField] private List<EntityDescriptionScriptableObject> _units;
        [SerializeField] private RectTransform _buttonContainer;
        [SerializeField] private SpawnButton _buttonPrefab;
        private List<SpawnButton> _createdButtons;
        
        public override void Init()
        {
            _createdButtons = new List<SpawnButton>();
            
            if (_entity.Has<SpawnComponent>())
            {
                ref var spawnComponent = ref _entity.GetComponent<SpawnComponent>();
                for (int i = 0; i < spawnComponent.PoolEntitys.Count; i++)
                {
                    _units.Add(spawnComponent.PoolEntitys[i]);    
                }
            }

            for (int i = 0; i < _units.Count; i++)
            {
                var button = Instantiate(_buttonPrefab, _buttonContainer);
                button.SetData(_units[i]);
                button.OnClick += OnClick;
                _createdButtons.Add(button);
            }            
        }

        public void DeInit()
        {
            for (int i = 0; i < _createdButtons.Count; i++)
            {
                _createdButtons[i].OnClick += OnClick;
                Destroy(_createdButtons[i].gameObject);
            }
        }
        
        private void Update()
        {
            if (_entity.IsNullOrDisposed())
            {
                Debug.Log("Игрок уничтожен");
                Destroy(gameObject);
                return;
            }
        }

        private void OnClick(EntityDescriptionScriptableObject data)
        {
            if (_entity.Has<SpawnComponent>())
            {
                ref var spawnComponent = ref _entity.GetComponent<SpawnComponent>();
                spawnComponent.QueueEntity.Add(Instantiate(data));
            }
        }
    }
}