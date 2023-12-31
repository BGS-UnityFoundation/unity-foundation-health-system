using UnityFoundation.Code.UnityAdapter;
using System;
using UnityEngine;

namespace UnityFoundation.HealthSystem
{
    [Obsolete]
    public sealed class HealthSystemMono : BilucaMono, IHealthSystem
    {
        [SerializeField] private bool setupOnAwake = false;

        [SerializeField] private float baseHealth;

        [SerializeField] private float currentHealth;

        public float BaseHealth => healthSystem.BaseHealth;
        public float CurrentHealth => healthSystem.CurrentHealth;
        public bool IsDead => healthSystem.IsDead;

        [SerializeField] private DamageableLayer layer;
        public DamageableLayer Layer {
            get { return layer; }
            set { layer = value; }
        }

        [field: SerializeField] public bool DestroyOnDied { get; set; } = false;

        public IHealthSystem HealthSystem => healthSystem;


        public event Action OnFullyHeal;
        public event Action OnDied;
        public event Action OnTakeDamage;
        public event Action<float> OnTakeDamageAmount;
        public event Action<float> OnHealthChanged;

        private DamageableLayerManager damageableLayerManager;
        private Func<bool> guardDamageCallback;

        private IHealthSystem healthSystem;

        protected override void OnAwake()
        {
            healthSystem = new HealthSystem();
            healthSystem.OnFullyHeal += FullyHealHandler;
            healthSystem.OnTakeDamage += TakeDamageHandler;
            healthSystem.OnTakeDamageAmount += TakeDamageAmountHandler;
            healthSystem.OnDied += DieHandler;

            OnObjectDestroyed += () => {
                healthSystem.OnFullyHeal -= FullyHealHandler;
                healthSystem.OnTakeDamage -= TakeDamageHandler;
                healthSystem.OnTakeDamageAmount -= TakeDamageAmountHandler;
                healthSystem.OnDied -= DieHandler;
            };

            if(setupOnAwake)
                Setup(baseHealth);
        }

        private void TakeDamageAmountHandler(float amount)
        {
            OnTakeDamageAmount?.Invoke(amount);
        }

        public void SetDamageableLayerManager(DamageableLayerManager manager)
        {
            damageableLayerManager = manager;
        }

        public void Setup(float baseHealth)
        {
            healthSystem.Setup(baseHealth);
        }

        private void FullyHealHandler()
        {
            OnFullyHeal?.Invoke();
        }

        private void TakeDamageHandler()
        {
            OnTakeDamage?.Invoke();
        }

        private void DieHandler()
        {
            OnDied?.Invoke();

            if(DestroyOnDied) Obj.Destroy();
        }

        public void SetGuardDamage(Func<bool> callback)
        {
            guardDamageCallback = callback;
        }

        public void Damage(float amount, DamageableLayer layer = null)
        {
            if(!CanInflictDamage(layer))
                return;

            healthSystem.Damage(amount, layer);
        }

        private bool CanInflictDamage(DamageableLayer layer)
        {
            if(IsDead)
                return false;

            if(
                damageableLayerManager != null
                && !damageableLayerManager.LayerCanDamage(layer, Layer)
            )
                return false;

            if(guardDamageCallback != null && guardDamageCallback())
                return false;

            return true;
        }

        public void Heal(float amount)
        {
            healthSystem.Heal(amount);
        }

        public void HealFull()
        {
            healthSystem.HealFull();
        }
    }
}