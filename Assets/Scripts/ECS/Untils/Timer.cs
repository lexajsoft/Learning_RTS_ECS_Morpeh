using System;

namespace ECS.Untils
{
    [Serializable]
    public struct Timer
    {
        public float Remain;
        public float Delayed;

        public bool UpdateAndCheck(float deltaTime)
        {
            if (Remain > 0)
            {
                Remain -= deltaTime;
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ResetTimer()
        {
            Remain = Delayed;
        }
    }
}