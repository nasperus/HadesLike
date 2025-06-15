using Stats;
using UnityEngine;

namespace Player.Skills
{
    public static class LifeSteal
    {

         static float _lifeStealPercent = 20f;

        public static bool IsEnabled { get; set; } = false;


        public static void GetLifeSteal(float damage, PlayerHealth playerHealth, StatCollection statCollection)
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
