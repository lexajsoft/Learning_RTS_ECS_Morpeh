using System;
using System.Collections.Generic;
using ECS.Components.Interface;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct AnimationComponent : IComponent, IComponentGameObjectInitting, ICloneable
    {
        public Animator Animator;
        
        public bool Inited => States != null;
        public Dictionary<string, int> States;
        public void Init(GameObject gameObject)
        {
            Animator = gameObject.GetComponent<Animator>();

            States = new Dictionary<string, int>();
            foreach (var parameter in Animator.parameters)
            {
                States.Add(parameter.name, parameter.nameHash);
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}