using System;

namespace UnityFoundation.HealthSystem
{
    public interface IHealthSystem : IDamageable, IHealable
    {
        float BaseHealth { get; }
        float CurrentHealth { get; }
        bool IsDead { get; }

        event Action OnDied;
        event Action<float> OnHealthChanged;

        void Setup(float baseHealth);
    }
}