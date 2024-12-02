using System;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Components
{
    [Serializable]
    public struct ProjectileTriggerEventRequestComponent : IComponent
    {
        // объект
        public Entity Entity;
        public GameObject GameObject;
        // объект в которого врезались
        public Entity OtherEntity;
        public GameObject OtherGameObject;
    }
}