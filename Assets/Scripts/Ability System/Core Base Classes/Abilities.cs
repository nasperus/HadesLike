using UnityEngine;

namespace Ability_System.Core_Base_Classes
{
    public abstract class Abilities: MonoBehaviour
    {
       
        protected BaseAbilityModifier Stats {get; private set;}

        protected void Init(BaseAbilityModifier modifier)
        {
            Stats = modifier;
        }
        
        protected virtual  float GetFinalDamage() => Stats.CurrentDamage;
        protected virtual float GetFinalRadius() => Stats.CurrentRadius;
        protected virtual float GetFinalAoeEffect() => Stats.CurrentArea;


    }
}
