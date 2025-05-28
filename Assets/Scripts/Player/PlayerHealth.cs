using System;
using Player.Skills;
using Stats;
using UnityEngine;
using TMPro;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, Enemy.IPlayerDamageable
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        [SerializeField] private PlayerArmor barrier;
        [SerializeField] private PlayerStatsManager statsManager;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        private void Start()
        {
         
            currentHealth = maxHealth;
        }
        

        public void UpdateMaxHealth()
        {
            maxHealth = statsManager.GetStatValue("Vitality");
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        public void TakeDamage(int damage)
        {
           var finalDamage = barrier.AbsorbDamage(damage);
            if (finalDamage > 0)
            {
                currentHealth -= finalDamage;
                if (currentHealth > 0)
                {
                   // Debug.Log($"hit: {currentHealth}");
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
