using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using IComponent = Scellecs.Morpeh.IComponent;
using Random = UnityEngine.Random;

namespace ECS.Components
{
    [Serializable]
    public struct SpawnPointsComponent : IComponent, ICloneable
    {
        public List<Transform> Points;
        [DefaultValue(0)]
        private int _nextSpawnPointIndex;

        public Transform UpdateAndGetNextSpawnPointIndex()
        {
            _nextSpawnPointIndex++;
            _nextSpawnPointIndex %= Points.Count;
            return GetCurrentSpawnPoint();
        }

        public Transform GetCurrentSpawnPoint()
        {
            return Points[_nextSpawnPointIndex];
        }

        public Vector3 GetRandomPoints()
        {
            if (Points.Count > 0)
            {
                return Points[Random.Range(0, Points.Count)].position;
            }

            return Vector3.zero;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}