using UnityEngine;

namespace Stats
{
    public class AddingStatValues : MonoBehaviour
    {
        [SerializeField] private PlayerStatsManager playerStatsManager;
        
        public void AddHaste(float value)
        {
            playerStatsManager.IncreaseBaseStat("Haste", value);
        }

        public void AddMastery(float value)
        {
            playerStatsManager.IncreaseBaseStat("Mastery", value);
        }

        public void AddCritical(float value)
        {
            playerStatsManager.IncreaseBaseStat("Critical", value);
        }

        public void AddVitality(float value)
        {
            playerStatsManager.IncreaseBaseStat("Vitality", value);
        }

        public void AddArmor(float value)
        {
            playerStatsManager.IncreaseBaseStat("Armor", value);
        }

        public void AddMana(float value)
        {
            playerStatsManager.IncreaseBaseStat("Mana", value);
        }

        public void AddMovementSpeed(float value)
        {
            playerStatsManager.IncreaseBaseStat("MovementSpeed", value);
        }
    }
}
