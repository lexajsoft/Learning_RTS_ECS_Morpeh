using UnityEngine;

namespace ECS.Untils
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        private Material _material;
        private int _healthValueProgressID = -1;
        public void Init()
        {
            _material = Instantiate(_meshRenderer.material);
            _meshRenderer.material = _material;
            _healthValueProgressID = Shader.PropertyToID("_Progress");
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        private float _value = 0;
        
        public void SetValue(float value)
        {
            if(_value == value)
                return;
            
            _value = value;
            value = Mathf.Clamp01(value);
            _material.SetFloat(_healthValueProgressID, value);
        }
    }
}