using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct DestroyAfterComponent : IComponent
    {
        public GameObject GameObject;
        public float Remain;

        public DestroyAfterComponent(float remain)
        {
            Remain = remain;
            GameObject = null;
        }
        
        public DestroyAfterComponent(float remain, GameObject gameObject)
        {
            Remain = remain;
            GameObject = gameObject;
        }
        
    }
}