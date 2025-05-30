/*
using UnityEngine;

namespace Stats
{
    public abstract class StatCalculator 
    {
        
        public static void CalculateStats(
            StatCollection statCollection,
            float baseDamage,
            float baseTickInterval,
            out float masteryTick,
            out float hasteTick,
            out float criticalChance)
        {
            if (statCollection == null)
            {
                Debug.LogWarning("StatCollection is null! Using default base values.");
                hasteTick = baseTickInterval;
                masteryTick = baseDamage;
                criticalChance = 0f;
                return;
            }
            
            var haste = statCollection.GetStatValue(StatTypeEnum.Haste);
            var mastery = statCollection.GetStatValue(StatTypeEnum.Mastery);
            var critical = statCollection.GetStatValue(StatTypeEnum.Critical);
            
            
            // Haste Calculation 1 Haste means 3% speed
            var flatReduction = haste * 0.02f;              
            var scalingMultiplier = 1f + (haste * 0.03f);    
            var adjustedTick = baseTickInterval - flatReduction;
            adjustedTick /= scalingMultiplier;
            hasteTick = Mathf.Max(0.05f, adjustedTick);        
            
            
            // Mastery Calculation 1 mastery means 1% damage
            var masteryMultiplier = GetMasteryMultiplier(mastery);
            masteryTick = baseDamage * masteryMultiplier;
            
            //Critical Calculation 1 crit means 1% crit chance
            criticalChance = Mathf.Clamp(critical / 100f, 0f, 0.95f);
            
            Debug.Log($"Stats Applied - Haste: {haste} (tick: {hasteTick:F2}s), " +
                      $"Mastery: {mastery} (damage: {masteryTick}), " +
                      $"Critical: {critical} (chance: {criticalChance:P0})");
        }
        private static float GetMasteryMultiplier(float mastery)
        {
            return 1f + (mastery * 0.01f);
        }
        
    }
}
*/
