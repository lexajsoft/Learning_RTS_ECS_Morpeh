using System;
using ECS.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ECS.Factories
{

    [Serializable]
    public struct ProjectileData
    {
        public int Damage;
        public Vector3 StartPosition;
        public Vector3 Direct;
        public float Speed;
        public GameObject Prefab;
    }

    public static class ProjectileFactory 
    {
        public static void CreateProjectile(ProjectileData projectileData, TagTeam TagTeam)
        {
            var projectileGameObject = GameObject.Instantiate(projectileData.Prefab, projectileData.StartPosition, Quaternion.identity);
            
#if UNITY_EDITOR
            var entityProvider = projectileGameObject.AddComponent<EntityProvider>();
            var projectileEntity = entityProvider.Entity;
#else
            var projectileEntity = WorldManager.WorldDefault.CreateEntity();
#endif
            
            projectileEntity.SetComponent(new TransformComponent()
            {
                Transform = projectileGameObject.transform
            });
            
            projectileEntity.SetComponent(new MovementComponent()
            {
                Direct = projectileData.Direct,
                Transform = projectileGameObject.transform,
                Rigidbody = projectileGameObject.GetComponent<Rigidbody>(),
                Speed = projectileData.Speed
            });
            
            projectileEntity.SetComponent(new PositionComponent()
            {
                Pos = projectileData.StartPosition
            });
            
            projectileEntity.SetComponent(new RotationComponent()
            {
                SpeedRotation = 1,
                DirectToLook = projectileData.Direct
            });
            
            projectileEntity.SetComponent(new ProjectileComponent()
            {
                Damage = projectileData.Damage,
                
            });
            projectileEntity.SetComponent(new TagTeamComponent()
            {
                TagTeam = TagTeam
            });
            
            projectileEntity.SetComponent(new DestroyAfterComponent()
            {
                GameObject = projectileGameObject,
                Remain = 5f // 5 sec
            });
            
            TriggerEventCollider triggerEventCollider = projectileGameObject.GetComponent<TriggerEventCollider>();
            if (triggerEventCollider == null)
                triggerEventCollider = projectileGameObject.AddComponent<TriggerEventCollider>();
            triggerEventCollider.SetEntity(projectileEntity);
            
        }
    }
}