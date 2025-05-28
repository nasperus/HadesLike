using UnityEngine;

namespace Ability_System.Core_Base_Classes
{
    public abstract class Abilities: MonoBehaviour
    {
       
        public BaseAbilityModifier Stats {get; private set;}

        public void Init(BaseAbilityModifier modifier)
        {
            Stats = modifier;
        }
        
        public virtual  float GetFinalDamage() => Stats.CurrentDamage;
        public virtual float GetFinalRadius() => Stats.CurrentRadius;
        public virtual float GetFinalAoeEffect() => Stats.CurrentArea;


    }
}
