using Player;
using Player.Skills;
using UnityEngine;

namespace Ability_System.Core_Base_Classes
{
    public class TestButton : MonoBehaviour
    {
        [SerializeField] private StrikeLightning strikeLightning;
        [SerializeField] private PlayerDebuff playerDebuff;
        [SerializeField] private AoeFireDamage aoeFireDamage;
        
       
        public void OnIncreaseLightningDamage()
        {
            if (strikeLightning != null)
            {
                strikeLightning.IncreaseLightningStrikeDamage(0.2f);
            }
        }

        public void OnInCreaseLightningRadius()
        {
            if (strikeLightning != null)
            {
                strikeLightning.IncreaseLightningStrikeRadius(0.4f);
            }
           
        }

        public void OnIncreaseDotDamage()
        {
            playerDebuff.IncreaseDotDamage(0.4f);
        }

        public void OnIncreaseAoeDamage()
        {
            aoeFireDamage.IncreaseAoeDamage(0.2f);
        }
    }
}
