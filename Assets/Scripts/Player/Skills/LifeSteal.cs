using Stats;
using UnityEngine;

namespace Player.Skills
{
    public class LifeSteal
    {

        private float _lifeStealPercent = 20f;

        public bool IsEnabled { get; set; } = true;


        public void GetLifeSteal(float damage, PlayerHealth playerHealth, StatCollection statCollection)
        {
            if (!IsEnabled) return;

           var damageAfterMastery = ApplyStatsToAbilities.ApplyMastery(damage, statCollection);
           var finalDamage = ApplyStatsToAbilities.ApplyCritChance(damageAfterMastery, statCollection);
            
            var healAmount = finalDamage * (_lifeStealPercent / 100f);
            playerHealth.CurrentHealth = Mathf.Clamp(playerHealth.CurrentHealth + healAmount, 
                0, playerHealth.MaxHealth);
            Debug.Log($"Final Damage: {finalDamage}, Heal: {healAmount}, HP: {playerHealth.CurrentHealth}");
        }
    }
}
