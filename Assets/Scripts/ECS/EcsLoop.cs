using System;
using ECS.Systems;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ECS
{
    public class EcsLoop : MonoBehaviour
    {
        public static EcsLoop Instance;
        
        private World _worldDefault;
        private SystemsGroup _worldDefaultSystemGroup;
        private World _worldEvent;
        private SystemsGroup _worldEventSystemGroup;

        private bool _isWork;

        private void Awake()
        {
            Instance = this;
        }

        public void Initialize()
        {
            WorldManager.CreateWorlds();

            // World - DEFAULT
            // установка систем для дефолтового мира
            _worldDefault = WorldManager.GetWorld(WorldManager.WORLD_DEFAULT);
            _worldDefaultSystemGroup = _worldDefault.CreateSystemsGroup();
            
            // Base System
            _worldDefaultSystemGroup.AddSystem(ScriptableObject.CreateInstance<HealthSystem>());
            _worldDefaultSystemGroup.AddSystem(ScriptableObject.CreateInstance<MovementSystem>());
            _worldDefaultSystemGroup.AddSystem(ScriptableObject.CreateInstance<RotationSystem>());
            
            // hp bar
            _worldDefaultSystemGroup.AddSystem(ScriptableObject.CreateInstance<HealthBarSystem>());
            
            // Spawn
            _worldDefaultSystemGroup.AddSystem(ScriptableObject.CreateInstance<SpawnSystem>());
           
            // Logic Units
            _worldDefaultSystemGroup.AddSystem(ScriptableObject.CreateInstance<WarriorLogicSystem>());
            _worldDefaultSystemGroup.AddSystem(ScriptableObject.CreateInstance<ArcherLogicSystem>());
            
            // Death
            _worldDefaultSystemGroup.AddSystem(ScriptableObject.CreateInstance<EntityDeathSystem>());
            
            // Destroy after time
            _worldDefaultSystemGroup.AddSystem(ScriptableObject.CreateInstance<DestroyAfterTimeSystem>());

            // Animation
            _worldDefaultSystemGroup.AddSystem(ScriptableObject.CreateInstance<AnimationSystem>());

            _worldDefaultSystemGroup.AddSystem(ScriptableObject.CreateInstance<GameOverSystem>());
            
            _worldDefaultSystemGroup.Initialize();
            
            CreateWorldViewer(_worldDefault);
            
            // World - EVENT            
            // установка систем для событийного мира
            _worldEvent = WorldManager.GetWorld(WorldManager.WORLD_EVENT);
            _worldEventSystemGroup = _worldEvent.CreateSystemsGroup();
            
            // Trigger / Collision
            _worldEventSystemGroup.AddSystem(ScriptableObject.CreateInstance<TriggerEventSystem>());
            
            // Visual Damage
            _worldEventSystemGroup.AddSystem(ScriptableObject.CreateInstance<VisualDamageSystem>());
            
            // damage / heal
            _worldEventSystemGroup.AddSystem(ScriptableObject.CreateInstance<DamageProcessingSystem>());
            _worldEventSystemGroup.AddSystem(ScriptableObject.CreateInstance<HealProcessingSystem>());
            
            
            // Find Target
            _worldEventSystemGroup.AddSystem(ScriptableObject.CreateInstance<NearEntitySystem>());
            _worldEventSystemGroup.AddSystem(ScriptableObject.CreateInstance<FindTargetSystem>());
            _worldEventSystemGroup.Initialize();
            
            CreateWorldViewer(_worldEvent);

            _isWork = true;
        }

        // создает объект на котором можно просматривать объекты
        private void CreateWorldViewer(World world)
        {
            var worldDefaultMono = new GameObject();
            worldDefaultMono.name = world.GetFriendlyName();
            worldDefaultMono.transform.SetParent(gameObject.transform);
            var worldViewer = worldDefaultMono.AddComponent<WorldViewer>();
            worldViewer.World = world;
        }

        private void Update()
        {   
            if(_isWork == false)
                return;
            
            _worldDefaultSystemGroup.Update(Time.deltaTime);
            _worldDefault.Commit();
            _worldEventSystemGroup.Update(Time.deltaTime);
            _worldEvent.Commit();
            
            // Update обновляет совершенно не то что подразумевалось
            //WorldManager.WorldDefault.Update(Time.deltaTime);
            //WorldManager.WorldDefault.Commit();
            //WorldManager.WorldEvent.Update(Time.deltaTime);
            //WorldManager.WorldEvent.Commit();
        }

        private void FixedUpdate()
        {
            if(_isWork == false)
                return;
            
            _worldDefaultSystemGroup.Update(Time.fixedDeltaTime);
            _worldEventSystemGroup.Update(Time.fixedDeltaTime);
            // WorldManager.WorldDefault.Update(Time.fixedDeltaTime);
            // WorldManager.WorldEvent.Update(Time.fixedDeltaTime);
        }


        public void GameOver()
        {
            _isWork = false;
        }
    }
}