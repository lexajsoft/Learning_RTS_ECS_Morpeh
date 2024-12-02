using System;
using ECS.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Factories
{
    public static class TriggerFactory
    {
        public static void CreateTriggerEventRequest(Entity entity, GameObject gameObject, Entity otherEntity, GameObject otherGameObject)
        {
            var entityEvent = WorldManager.WorldEvent.CreateEntity();
            entityEvent.SetComponent(new ProjectileTriggerEventRequestComponent()
            {
                Entity = entity,
                GameObject = gameObject,
                OtherEntity = otherEntity,
                OtherGameObject = otherGameObject
            });
        }
    }
}