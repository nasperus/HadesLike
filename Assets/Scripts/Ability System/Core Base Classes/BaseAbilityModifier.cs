using UnityEngine;

namespace Ability_System.Core_Base_Classes
{
    public class BaseAbilityModifier
    {
        public string Name;
        public float BaseDamage;
        public float BaseRadius;
        public float BaseAreaOfEffect;

         public StatModifier Multipliers  = new StatModifier();
        
        public float CurrentDamage => BaseDamage * Multipliers.DamageMultiplier;
        public float CurrentRadius => BaseRadius * Multipliers.RadiusMultiplier;
        public float CurrentArea => BaseAreaOfEffect * Multipliers.AoEMultiplier;

        public BaseAbilityModifier(float baseDamage, float baseRadius, float baseAoe)
        {
            BaseDamage = baseDamage;
            BaseRadius = baseRadius;
            BaseAreaOfEffect = baseAoe;
        }
        
        public void Apply(StatModifier other)
        {
            Multipliers.Apply(other);
        }
        
        public void Reset()
        {
            Multipliers.Reset();
        }

    }
}
