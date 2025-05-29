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
            var armorValue = playerStatsManager.GetStatValue(StatTypeEnum.Armor);
            armorValue = Mathf.Clamp(armorValue, 0, 100);

            var damageAfterArmor = incomingDamage * (1f - armorValue / 100f);
            var reducedDamage = Mathf.CeilToInt(damageAfterArmor);
            return reducedDamage;
        }
    }
}
