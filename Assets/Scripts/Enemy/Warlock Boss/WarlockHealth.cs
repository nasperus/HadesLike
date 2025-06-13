using System;
using Enemy.Archer;
using TMPro;
using UnityEngine;

namespace Enemy.Warlock_Boss
{
    public class WarlockHealth : MonoBehaviour, IEnemyDamageable
    {
        
        [SerializeField] private float warlockHealth;
        [SerializeField] private TextMeshProUGUI  warlockHealthText;


        private void Update()
        {
            warlockHealthText.text = $"Warlock Health: {warlockHealth}";
        }

        public void TakeDamage(float amount)
        {
            warlockHealth -= amount;

            if (warlockHealth <= 0)
            {
                Debug.Log("Warlock Dead");
            }
           
        }
    }
}
