using UnityEngine;

namespace Ability_System.Core_Base_Classes
{
    public static class AbilityFactory
    {
        public static BaseAbilityModifier StrikeLightning(float skillDamage, float radius)
        {
            return new BaseAbilityModifier(
                "Lightning Strike",
                skillDamage,
                radius,
                0f 
            );
        }

        public static BaseAbilityModifier DotDamage(float skillDamage)
        {
            return new BaseAbilityModifier(
                "Dot Damage",
                skillDamage,
                0f,
                0f
            );
        }

        public static BaseAbilityModifier AoeFireDamage(float skillDamage, float radius)
        {
            return new BaseAbilityModifier(
                "Aoe Fire Damage",
                skillDamage,
                radius,
                0f
            );
        }

        public static BaseAbilityModifier AutoAttackDamage(float attackDamage, float radius)
        {
            return new BaseAbilityModifier(
                name: "Auto Attack",
                attackDamage,
                radius,
                0f);
        }

        public static BaseAbilityModifier JavelinDamage(float skillDamage, float radius)
        {
            return new BaseAbilityModifier(
                name: "Throw Javelin",
                skillDamage,
                radius,
                0f
            );
        }

      
        
    }
}
