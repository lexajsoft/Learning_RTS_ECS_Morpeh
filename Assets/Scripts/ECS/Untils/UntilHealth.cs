using ECS.Components;

namespace ECS.Untils
{
    public static class UntilHealth
    {
        public static void Damage(this ref HealthComponent healthComponent, int damage)
        {
            healthComponent.HealthCurrent -= damage;
            if (healthComponent.HealthCurrent < 0)
                healthComponent.HealthCurrent = 0;
        }
        
        public static void Heal(this ref HealthComponent healthComponent, int heal)
        {
            healthComponent.HealthCurrent += heal;
            if (healthComponent.HealthCurrent > healthComponent.HealthMax)
                healthComponent.HealthCurrent = healthComponent.HealthMax;
        }
    }
}