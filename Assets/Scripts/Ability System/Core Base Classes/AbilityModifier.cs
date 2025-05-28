using UnityEngine;

namespace Ability_System.Core_Base_Classes
{
    public class AbilityModifier
    {
        public string Name;
        public float BaseDamage;
        public float BaseRange;
        public float BaseAreaOfEffect;

        public float DamageMultiplier = 1f;
        public float RangeMultiplier = 1f;
        public float AreaMultiplier = 1f;
        
        public float CurrentDamage => BaseDamage * DamageMultiplier;
        public float CurrentRange => BaseRange * RangeMultiplier;
        public float CurrentArea => BaseAreaOfEffect * AreaMultiplier;

        public AbilityModifier(float baseDamage, float baseRange, float baseAoe)
        {
            BaseDamage = baseDamage;
            BaseRange = baseRange;
            BaseAreaOfEffect = baseAoe;
        }

    }
}
