using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityFoundation.HealthSystem.Samples
{
    public class UISample : MonoBehaviour
    {
        [SerializeField] HealthSystemMono healthSystem;
        [SerializeField] Button damageButton;
        
        public void Start()
        {
            damageButton.onClick.AddListener(() => {
                healthSystem.Damage(1f);
            });
        }

    }
}
