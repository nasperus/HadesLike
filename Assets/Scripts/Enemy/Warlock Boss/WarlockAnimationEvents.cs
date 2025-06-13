using UnityEngine;

namespace Enemy.Warlock_Boss
{
    public class WarlockAnimationEvents : MonoBehaviour
    {
        
       [SerializeField] private WarlockStateMachine warlockState;
       

        public void TriggerSkills()
        {
            warlockState.UseCurrentSkill();
        }
        
    }
}
