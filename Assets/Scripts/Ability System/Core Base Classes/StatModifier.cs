using UnityEngine;

namespace Ability_System.Core_Base_Classes
{
    public class StatModifier
    {
        public float DamageMultiplier = 1f;
        public float RadiusMultiplier = 1f;
        public float AoEMultiplier = 1f;

        public void Reset()
        {
            DamageMultiplier = 1f;
            RadiusMultiplier = 1f;
            AoEMultiplier = 1f;
        }

        public void Apply(StatModifier other)
        {
            DamageMultiplier *= other.DamageMultiplier;
            RadiusMultiplier *= other.RadiusMultiplier;
            AoEMultiplier *= other.AoEMultiplier;
        }
    }
}
