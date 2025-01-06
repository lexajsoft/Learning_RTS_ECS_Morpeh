using UnityEngine;

namespace ECS.Untils
{
    public class UnitHealthBar : ProgressBar
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            transform.rotation = Quaternion.Euler(-1 * _mainCamera.transform.rotation.eulerAngles.x,-1 * _mainCamera.transform.rotation.eulerAngles.y,0);
        }
    }
}