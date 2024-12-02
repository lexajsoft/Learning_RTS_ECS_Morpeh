using UnityEngine;

namespace ECS.Untils
{
    public class UnitHealthBar : ProgressBar
    {
        private void Update()
        {

            //transform.LookAt(transform.position + transform.forward);// Camera.main.transform.position);
            Camera mainCamera = Camera.main; // Получаем основную камеру
            
            transform.rotation = Quaternion.Euler(-1 * mainCamera.transform.rotation.eulerAngles.x,-1 * mainCamera.transform.rotation.eulerAngles.y,0);
            // if (mainCamera != null)
            // {
            //     Vector3 direction = mainCamera.transform.position - transform.position; // Направление к камере
            //     direction.y = 0; // Обнуляем вертикальную составляющую, чтобы объект оставался горизонтальным
            //     transform.rotation = Quaternion.LookRotation(direction); // Поворачиваем объект к камере
            // }
        }
    }
}