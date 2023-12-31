using System;
using UnityEngine;
using UnityFoundation.Code;

namespace UnityFoundation.HealthSystem
{
    public class HealthSystem : IHealthSystem
    {
        private ValueEvaluation<float> currentHealthEval;

        public float BaseHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }
        public DamageableLayer Layer { get; private set; }

        public event Action OnFullyHeal;
        public event Action OnDied;
        public event Action OnTakeDamage;
        public event Action<float> OnTakeDamageAmount;
        public event Action<float> OnHealthChanged;

        public void Damage(float amount, DamageableLayer layer = null)
        {
            var damageAmount = EvaluateDamage(amount);
            UpdateHealth(damageAmount);

            if(!IsDead)
            {
                OnTakeDamage?.Invoke();
                OnTakeDamageAmount?.Invoke(damageAmount);
            }
        }

        public void Heal(float amount)
        {
            UpdateHealth(Mathf.Abs(amount));
        }

        public void HealFull()
        {
            UpdateHealth(BaseHealth);
        }

        public void Setup(float baseHealth)
        {
            BaseHealth = baseHealth;
            CurrentHealth = baseHealth;

            currentHealthEval = ValueEvaluation<float>.Create(() => CurrentHealth);
            currentHealthEval
                .If((h) => h <= 0f)
                .Do(() => { IsDead = true; OnDied?.Invoke(); });

            currentHealthEval
                .If((h) => h == BaseHealth)
                .Do(() => OnFullyHeal?.Invoke());
        }

        public void SetDamageableLayer(DamageableLayer layer)
        {
            Layer = layer;
        }

        protected virtual float EvaluateDamage(float amount)
        {
            return -Mathf.Abs(amount);
        }

        private void UpdateHealth(float amount)
        {
            if(IsDead) return;

            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, BaseHealth);
            currentHealthEval.Eval();
            OnHealthChanged?.Invoke(CurrentHealth);
        }
    }
}