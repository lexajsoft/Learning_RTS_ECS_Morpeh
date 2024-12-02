using System.Linq;
using ECS.Components;
using ECS.Factories;
using Extension;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(SpawnSystem))]
    public sealed class SpawnSystem : UpdateSystem
    {
        /*
     * Как вариант можно разделить на 3 системы спавна
     * 1. система которая ищет объекты которые умеют создавать юнитов, и дает им задачу их создавать
     * 2. система которая обрабатывает процеес постройки
     * 3. система которая создает
     */
    
        // Для поиск объектов которые ничего не спавнят
        private Filter _spawnFindFilter;

        // Для просчета времени 
        private Filter _spawnProcess;    
        private Filter _spawnReady;
        public override void OnAwake() 
        {
            World = WorldManager.WorldDefault;
            _spawnFindFilter = World.Filter
                .With<SpawnComponent>()
                .With<SpawnPointsComponent>()
                .Without<EntitySpawnProccesingComponent>()
                .Without<EntitySpawnReadyComponent>()
                .With<TagTeamComponent>()
                .Build();
        
            _spawnProcess = World.Filter
                .With<EntitySpawnProccesingComponent>()
                .Build();
        
            _spawnReady= World.Filter
                .With<SpawnPointsComponent>()
                .With<EntitySpawnReadyComponent>()
                .With<TagTeamComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime) 
        {
            // при первом запуске создает определенное количество юнитов
            foreach (var entity in _spawnFindFilter)
            {
                ref var spawnComponent = ref entity.GetComponent<SpawnComponent>();
                ref var tagTeamComponent = ref entity.GetComponent<TagTeamComponent>();
                if (spawnComponent.PoolEntitys.Count > 0)
                {
                    // чисто стартовая застройка юнитов
                    if (spawnComponent.UnitQuickSpawnCount > 0)
                    {
                        ref var spawnPointsComponent = ref entity.GetComponent<SpawnPointsComponent>();

                        for (int i = 0; i < spawnComponent.UnitQuickSpawnCount; i++)
                        {
                            var entityDescriptionScriptableObject = spawnComponent.PoolEntitys.GetRandom();
                            var spawnPointTransform = spawnPointsComponent.UpdateAndGetNextSpawnPointIndex();
                            var position = spawnPointTransform.position;
                            var rotation = spawnPointTransform.rotation;
                            
                            EntityFactory.CreateEntity(
                                entityDescriptionScriptableObject.GetEntityDescriptionData(),
                                entityDescriptionScriptableObject.GetEntityComponentsData(),
                                null, tagTeamComponent.TagTeam, position, rotation);
                        }

                        spawnComponent.UnitQuickSpawnCount = 0;
                    }

                    // автоматическии дает задачу на создание рандомного нпс
                    // вешает на объект процесc построения нпс
                    // entity.SetComponent(new EntitySpawnProccesingComponent(spawnComponent.PoolEntitys.GetRandom()));
                }

                if (spawnComponent.QueueEntity.Count > 0)
                {
                    var entityDescriptionScriptableObject = ScriptableObject.Instantiate(spawnComponent.QueueEntity[0]);
                    entity.SetComponent(new EntitySpawnProccesingComponent(entityDescriptionScriptableObject));
                    spawnComponent.QueueEntity.RemoveAt(0);
                    Debug.Log($"[{tagTeamComponent.TagTeam.ToString()}]Начался процес пострйоки юнита:" + entityDescriptionScriptableObject.GetEntityDescriptionData().Name);
                }
            }
            
            // обновляем процесс постройки юнита
            foreach (var entity in _spawnProcess)
            {
                ref var npcSpawnProcessingComponent = ref entity.GetComponent<EntitySpawnProccesingComponent>();
                if (npcSpawnProcessingComponent.Remaing > 0)
                {
                    npcSpawnProcessingComponent.Remaing -= deltaTime;
                }
                else
                {
                    entity.SetComponent(new EntitySpawnReadyComponent(npcSpawnProcessingComponent.entityDescriptionScriptableObject));
                    entity.RemoveComponent<EntitySpawnProccesingComponent>();
                }
            }
        
            // процесс спавна юнита
            foreach (var entity in _spawnReady)
            {
                ref var spawnPointsComponent = ref entity.GetComponent<SpawnPointsComponent>();
                ref var entitySpawnReadyComponent = ref entity.GetComponent<EntitySpawnReadyComponent>();
                ref var tagTeamComponent = ref entity.GetComponent<TagTeamComponent>();
            
                var spawnPointTransform = spawnPointsComponent.UpdateAndGetNextSpawnPointIndex();

                var position = spawnPointTransform.position;
                var rotation = spawnPointTransform.rotation;

                EntityFactory.CreateEntity(
                    entitySpawnReadyComponent.entityDescriptionScriptableObject.GetEntityDescriptionData(),
                    entitySpawnReadyComponent.entityDescriptionScriptableObject.GetEntityComponentsData(),
                    null, tagTeamComponent.TagTeam, position, rotation);
            
                Debug.Log($"[{tagTeamComponent.TagTeam.ToString()}] Юнит построен:" + entitySpawnReadyComponent.entityDescriptionScriptableObject.GetEntityDescriptionData().Name);
                entity.RemoveComponent<EntitySpawnReadyComponent>();
            }
        }
    }
}