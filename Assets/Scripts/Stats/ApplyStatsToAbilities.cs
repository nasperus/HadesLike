using UnityEngine;

namespace Stats
{
    public static class ApplyStatsToAbilities
    {
        public static float ApplyMastery(float baseDamage, StatCollection stats)
        {
            // Each point of Mastery increases 1% total Damage
            //1 = 1% Mastery
            var mastery = stats.GetStatValue("Mastery");
            return baseDamage * (1f + mastery * 0.01f);
        }

        public static float ApplyCritChance(float baseDamage, StatCollection stats)
        {
            // Each point of Critical grants 1% critical hit chance
            //1=1% crit
            var critical = stats.GetStatValue("Critical");
            var criticalChance = Mathf.Clamp(critical / 100f, 0f, 0.95f);

            if (Random.value <= criticalChance)
            {
                return baseDamage * 2f; // Critical hit deals double damage
            }
            return baseDamage;
        }

        public static float ApplyHasteSpeed(float baseHaste, StatCollection stats)
        {
            // Each point of Haste increases speed by 3% reducing tick rate and cooldowns.
            //1 = 3% Haste
            var haste = stats.GetStatValue("Haste");
            var adjustedTick = baseHaste / (1 + haste * 0.03f);
            return Mathf.Max(0.30f, adjustedTick); // Clamp to minimum value
        }
        
        public static float ApplyHasteDuration(float baseDuration, StatCollection stats)
        {
            // Each point of Haste increases ability duration by 2%
            //1 = 2% Haste
            var haste = stats.GetStatValue("Haste");
            var bonusDuration = baseDuration * (haste * 0.02f);
            return baseDuration + bonusDuration;
        }
        
        public static float ApplyHasteCastAndAttackSpeed(StatCollection stats)
        {
            // Each point of Haste increases attack/cast animation speed by 2%
            // 1 = 2% Haste
            var haste = stats.GetStatValue("Haste");
            return 1f + (haste * 0.02f);
        }

        public static float ApplyMovementSpeedBonus(float baseSpeed, StatCollection stats)
        {
            // Each point of MovementSpeed increases speed by 1%
            //1 = 1% Speed
            var movementSpeed = stats.GetStatValue("MovementSpeed");
            var multiplier = 1f + (movementSpeed * 0.01f);
            return baseSpeed * multiplier;
        }

  
    }
}
