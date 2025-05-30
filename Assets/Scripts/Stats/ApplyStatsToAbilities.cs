using UnityEngine;

namespace Stats
{
    public static class ApplyStatsToAbilities
    {
        public static float ApplyMastery(float baseDamage, StatCollection stats)
        {
            // Each point of Mastery increases 1% total Damage
            //1 = 1% Mastery
            var mastery = stats.GetStatValue(StatTypeEnum.Mastery);
            return baseDamage * (1f + mastery * 0.01f);
        }

        public static float ApplyCritChance(float baseDamage, StatCollection stats)
        {
            // Each point of Critical grants 1% critical hit chance
            //1=1% crit
            var critical = stats.GetStatValue(StatTypeEnum.Critical);
            var criticalChance = Mathf.Clamp(critical / 100f, 0f, 0.95f);

            if (Random.value <= criticalChance)
            {
                return baseDamage * 2f; // Critical hit deals double damage
            }
            return baseDamage;
        }

        public static float ApplyHasteSpeed(float baseHaste, StatCollection stats)
        {
            // Each point of Haste increases speed by 1.5% reducing tick rate and cooldowns.
            //1 = 1.5% Haste
            var haste = stats.GetStatValue(StatTypeEnum.Haste);
            var adjustedTick = baseHaste / (1 + haste * 0.015f);
            return Mathf.Max(0.30f, adjustedTick); // Clamp to minimum value
        }
        public static float ApplyArmorReduction(float incomingDamage, StatCollection stats)
        {
            // Each point of Armor reduces incoming damage by 1%, capped at 65%
            var armor = stats.GetStatValue(StatTypeEnum.Armor);
            var armorValue = Mathf.Clamp(armor, 0, 65); // Use your defined cap
            return incomingDamage * (1f - armorValue / 100f);
        }
        
        public static float ApplyHasteDuration(float baseDuration, StatCollection stats)
        {
            // Each point of Haste increases ability duration by 2%
            //1 = 2% Haste
            var haste = stats.GetStatValue(StatTypeEnum.Haste);
            var bonusDuration = baseDuration * (haste * 0.02f);
            return baseDuration + bonusDuration;
        }
        
        public static float ApplyHasteCastAndAttackSpeed(StatCollection stats)
        {
            // Each point of Haste increases attack/cast animation speed by 2%
            // 1 = 2% Haste
            var haste = stats.GetStatValue(StatTypeEnum.Haste);
            return 1f + (haste * 0.02f);
        }

        public static float ApplyMovementSpeedBonus(float baseSpeed, StatCollection stats)
        {
            // Each point of MovementSpeed increases speed by 1%
            //1 = 1% Speed
            var movementSpeed = stats.GetStatValue(StatTypeEnum.MovementSpeed);
            var multiplier = 1f + (movementSpeed * 0.01f);
            return baseSpeed * multiplier;
        }
        
        public static float ApplyVitalityBonus(float baseHealth, StatCollection stats)
        {
            // Each point of Vitality increases max health by 1%
            // 1 = 1% health increase, capped at 300%
            var vitality = stats.GetStatValue(StatTypeEnum.Vitality);
            return baseHealth * (1f + vitality * 0.01f);
        }
        
        public static float ApplyManaBonus(float baseMana, StatCollection stats)
        {
            // Each point of Mana increases max mana by 1%
            // 1 = 1% mana increase, capped at 300%
            var mana = stats.GetStatValue(StatTypeEnum.Mana);
            return baseMana * (1f + mana * 0.01f);
        }

  
    }
}
