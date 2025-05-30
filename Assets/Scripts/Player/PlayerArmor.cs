using Stats;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerArmor : MonoBehaviour
    {
        [SerializeField] private PlayerAnimations playerAnimations;
        [SerializeField] private PlayerStatsManager playerStatsManager;
        [SerializeField] private float armorValues;
        
        
        public int AbsorbDamage(int incomingDamage)
        {
            var damageAfterArmor = ApplyStatsToAbilities.ApplyArmorReduction(incomingDamage, playerStatsManager.GetStatCollection());
            return Mathf.CeilToInt(damageAfterArmor);
        }
    }
}
