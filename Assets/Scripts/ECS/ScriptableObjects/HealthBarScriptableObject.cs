using UnityEngine;

namespace ECS.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create/HealthBarScriptableObject", fileName = "HealthBarScriptableObject", order = 0)]
    public class HealthBarScriptableObject : ScriptableObject
    {
        [SerializeField] private GameObject _healthBarPrefab;
        [SerializeField] private Vector3 _offsetInstall;
        [SerializeField] private Vector3 _rotationInstall;

        public GameObject GetPrefab() => _healthBarPrefab;
        public Vector3 GetOffsetInstall() => _offsetInstall;

        public Quaternion GetRotationInstall()
        {
            return Quaternion.Euler(_rotationInstall);
        }
    }
}