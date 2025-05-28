using Player.Skills;
using UnityEngine;

namespace Ability_System.Core_Base_Classes
{
    public class TestButton : MonoBehaviour
    {
        [SerializeField] private StrikeLightning strikeLightning;
        [SerializeField] private float damageIncreaseAmount = 0.5f; 
        [SerializeField] private float rangeIncreaseAmount = 0.3f;  
        [SerializeField] private float aoeIncreaseAmount = 0.25f; 
       
        public void OnIncreaseDamage()
        {
            if (strikeLightning != null)
            {
                strikeLightning.IncreaseDamage(0.2f);
            }
        }

        public void OnInCreaseRadius()
        {
            if (strikeLightning != null)
            {
                strikeLightning.IncreaseRadius(0.4f);
            }
           
        }
    }
}
