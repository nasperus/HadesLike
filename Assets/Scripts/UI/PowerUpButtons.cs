using System;
using Player;
using Player.Skills;
using Stats;
using UnityEngine;

namespace UI
{
    public class PowerUpButtons : MonoBehaviour
    {
        private AddingStatValues _addStatValues;
        private PlayerStatsManager _playerStatsManager;
        private PlayerHealth _playerHealth;
        private PlayerArmor _playerArmor;
        private PlayerMana _playerMana;
        private PlayerMovement _playerMovement;
        private StatCollection _statCollection;

       
       public void SetPlayerReferences(GameObject player)
       {
           _addStatValues = player.GetComponent<AddingStatValues>();
           _playerStatsManager = player.GetComponent<PlayerStatsManager>();
           _playerHealth = player.GetComponent<PlayerHealth>();
           _playerArmor = player.GetComponent<PlayerArmor>();
           _playerMana = player.GetComponent<PlayerMana>();
           _playerMovement = player.GetComponent<PlayerMovement>();
           _statCollection = player.GetComponent<StatCollection>();
       }
     

       public void OnClickHaste()
       {
           _addStatValues.AddHaste(2f);
           //trigger.OnPowerUpChosen();
       }
       
       public void OnClickMastery()
       {
           _addStatValues.AddMastery(5f); 
          // trigger.OnPowerUpChosen();
          
       }
       
       public void OnClickCrit()
       {
           _addStatValues.AddCritical(3f); 
           //trigger.OnPowerUpChosen();
       }
       public void OnClickHP()
       {
           _addStatValues.AddVitality(15f); 
           _playerHealth.UpdateMaxHealth(); 
       }

       public void OnClickArmor()
       {
           _addStatValues.AddArmor(5f);
           
       }

       public void OnClickMana()
       {
           _addStatValues.AddMana(10f);
           _playerMana.UpdateMaxMana();
       }

       public void OnClickMovement()
       {
           _addStatValues.AddMovementSpeed(1.5f);
           _playerMovement.UpdateMovementSpeed();
       }


       public void OnClickReset()
       {
           _playerStatsManager.ResetBonuses(StatTypeEnum.Haste);
           _playerStatsManager.ResetBonuses(StatTypeEnum.Mastery);
           _playerStatsManager.ResetBonuses(StatTypeEnum.Critical);
           _playerStatsManager.ResetBonuses(StatTypeEnum.Vitality);
           _playerStatsManager.ResetBonuses(StatTypeEnum.Armor);
           _playerStatsManager.ResetBonuses(StatTypeEnum.Mana);
           _playerStatsManager.ResetBonuses(StatTypeEnum.MovementSpeed);
           _playerMana.UpdateMaxMana();
           _playerHealth.UpdateMaxHealth(); 
           _playerMovement.UpdateMovementSpeed();
       }

        
    }
}
