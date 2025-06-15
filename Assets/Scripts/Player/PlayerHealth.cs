using System;
using Enemy.Mutant;
using Player.Skills;
using Stats;
using UnityEngine;
using TMPro;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IPlayerDamageable
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        [SerializeField] private PlayerArmor barrier;
        [SerializeField] private PlayerStatsManager statsManager;
        [SerializeField] private PlayerDash playerDash;
       
        public float MaxHealth => maxHealth;
        private float _originalMaxHealth;
        public float CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = value;
        }

        private void Start()
        {
            _originalMaxHealth = maxHealth;
            currentHealth = maxHealth;
        }
        

        public void UpdateMaxHealth()
        {
            // Always use the original value for calculation
            maxHealth = ApplyStatsToAbilities.ApplyVitalityBonus(_originalMaxHealth, statsManager.GetStatCollection());
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
        
        public void TakeDamage(int damage)
        {
           var finalDamage = barrier.AbsorbDamage(damage);

           if (playerDash.IsDashing) return;
           
            if (finalDamage > 0)
            {
                currentHealth -= finalDamage;
                if (currentHealth > 0)
                {
                    
                }
                else
                {
                   // Debug.Log("dead");
                }
            }
        }

        public void Heal(float healAmount)
        {
            currentHealth += healAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            //Debug.Log($"Healed: {healAmount}, Current Health: {currentHealth}");
        }
    }
}
