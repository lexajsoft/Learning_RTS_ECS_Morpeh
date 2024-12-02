using System;
using System.Collections.Generic;
using Core;
using ECS.Factories.Data;
using ECS.ScriptableObjects;
using UnityEngine;

namespace ECS.Factories
{
    [Serializable]
    public enum AttackType
    {
        SwordBite,
        ArrowBite
    }

    public static class VisualEffectFactory
    {
        private static VisualEffectCollectionScriptableObject _visualEffectCollectionScriptableObject;
        private static Dictionary<AttackType, VisualEffectData> _collection;
        private static Transform _container = null;
        
        public static void Init(Transform container = null)
        {
            _container = container;
            
            _collection = new Dictionary<AttackType, VisualEffectData>();
            _visualEffectCollectionScriptableObject = ServiceLocator.Get<VisualEffectCollectionScriptableObject>();
            for (int i = 0; i < _visualEffectCollectionScriptableObject.List.Count; i++)
            {
                _collection.Add(_visualEffectCollectionScriptableObject.List[i].AttackType, _visualEffectCollectionScriptableObject.List[i]);
            }
        }

        public static void CreateVisual(Vector3 position, AttackType AttackType)
        {
            if (_collection.ContainsKey(AttackType))
            {
                var data = _collection[AttackType];
                var effect = GameObject.Instantiate(data.Prefab,position, Quaternion.identity, _container);
                GameObject.Destroy(effect, data.LiveTime);
            }
        }
    }
}