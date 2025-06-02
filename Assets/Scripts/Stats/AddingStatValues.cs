using System;
using UnityEngine;

namespace Stats
{
    public class AddingStatValues : MonoBehaviour
    {
        [SerializeField] private PlayerStatsManager playerStatsManager;

       
        public void AddHaste(float value)
        {
            playerStatsManager.IncreaseBaseStat(StatTypeEnum.Haste, value);
        }

        public void AddMastery(float value)
        {
            playerStatsManager.IncreaseBaseStat(StatTypeEnum.Mastery, value);
        }

        public void AddCritical(float value)
        {
            playerStatsManager.IncreaseBaseStat(StatTypeEnum.Critical, value);
        }

        public void AddVitality(float value)
        {
            playerStatsManager.IncreaseBaseStat(StatTypeEnum.Vitality, value);
        }

        public void AddArmor(float value)
        {
            playerStatsManager.IncreaseBaseStat(StatTypeEnum.Armor, value);
        }

        public void AddMana(float value)
        {
            playerStatsManager.IncreaseBaseStat(StatTypeEnum.Mana, value);
        }

        public void AddMovementSpeed(float value)
        {
            playerStatsManager.IncreaseBaseStat(StatTypeEnum.MovementSpeed, value);
        }
    }
}
