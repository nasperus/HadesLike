using UnityEngine;

namespace Ability_System.Core_Base_Classes
{
    public abstract class Abilities: MonoBehaviour
    {
       
        public AbilityModifier Stats {get; private set;}

        public void Init(AbilityModifier modifier)
        {
            Stats = modifier;
        }
        
        public virtual  float GetFinalDamage() => Stats.CurrentDamage;
        public virtual float GetFinalRange() => Stats.CurrentRange;
        public virtual float GetFinalAoeEffect() => Stats.CurrentArea;


    }
}
