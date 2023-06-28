using UnityEngine;

namespace UnityFoundation.HealthSystem
{
    public class HealthUpdater : MonoBehaviour
    {
        public void Start()
        {
            var healthSystem = GetComponent<HealthSystemMono>();
            var healthBar = GetComponent<IHealthBar>();

            healthBar.Setup(healthSystem.HealthSystem.BaseHealth);

            healthSystem.HealthSystem.OnHealthChanged
                += (currentHealth) => healthBar.SetCurrentHealth(currentHealth);
        }
    }
}
