using System;
using UnityEngine;

public enum StatType
{
    Damage,
    Radius,
    AoE
}

namespace Ability_System.Core_Base_Classes
{
    public static class BaseAbilityModifierExtension
    {
        public static void IncreaseStats(this BaseAbilityModifier stats, StatType statType, float multiplier)
        {
            var modifier = new StatModifier();

            switch (statType)
            {
                case StatType.Damage:
                    modifier.DamageMultiplier = 1 + multiplier;
                    break;
                case StatType.Radius:
                    modifier.RadiusMultiplier = 1 + multiplier;
                    break;
                case StatType.AoE:
                    modifier.AoEMultiplier = 1 + multiplier;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
            
            stats.Apply(modifier);
            var message = statType switch
            {
                StatType.Damage => $"[IncreaseStat] Ability: {stats.Name}, Stat: Damage, Base: {stats.BaseDamage}, " +
                                   $"Multiplier: {stats.Multipliers.DamageMultiplier}, Final: {stats.CurrentDamage}",
                StatType.Radius => $"[IncreaseStat] Ability: {stats.Name}, Stat: Radius, Base: {stats.BaseRadius}, " +
                                   $"Multiplier: {stats.Multipliers.RadiusMultiplier}, Final: {stats.CurrentRadius}",
                StatType.AoE => $"[IncreaseStat] Ability: {stats.Name}, Stat: AoE, Base: {stats.BaseAreaOfEffect}, " +
                                $"Multiplier: {stats.Multipliers.AoEMultiplier}, Final: {stats.CurrentArea}",
                _ => $"[IncreaseStat] Unknown stat"
            };
            Debug.Log(message);
        }

        // public static void IncreaseDotDamage(this BaseAbilityModifier stats, float multiplier)
        // {
        //     var dotDamageIncrease = new StatModifier
        //     {
        //         DamageMultiplier = 1 + multiplier
        //     };
        //     stats.Apply(dotDamageIncrease);
        //     Debug.Log($"[Damage Increase] Base: {stats.BaseDamage}, Multiplier: " +
        //               $"{stats.Multipliers.DamageMultiplier}, Final: {stats.CurrentDamage}");
        // }
        //
       
    }
}
