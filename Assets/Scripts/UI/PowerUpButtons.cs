using Player;
using Player.Skills;
using Stats;
using UnityEngine;

namespace UI
{
    public class PowerUpButtons : MonoBehaviour
    {
       [SerializeField] private AddingStatValues addStatValues;
       [SerializeField] private PlayerStatsManager playerStatsManager;
       [SerializeField] private PowerUpTrigger trigger;
       [SerializeField] private PlayerHealth playerHealth;
       [SerializeField] private PlayerArmor playerArmor;
       [SerializeField] private PlayerMana playerMana;
       [SerializeField] private PlayerMovement playerMovement;
       
     
       public void OnClickHaste()
       {
           addStatValues.AddHaste(2f);
           //trigger.OnPowerUpChosen();
       }
       
       public void OnClickMastery()
       {
           addStatValues.AddMastery(5f); 
          // trigger.OnPowerUpChosen();
          
       }
       
       public void OnClickCrit()
       {
           addStatValues.AddCritical(3f); 
           //trigger.OnPowerUpChosen();
       }
       public void OnClickHP()
       {
           addStatValues.AddVitality(15f); 
           playerHealth.UpdateMaxHealth(); 
       }

       public void OnClickArmor()
       {
           addStatValues.AddArmor(5f);
           
       }

       public void OnClickMana()
       {
           addStatValues.AddMana(10f);
           playerMana.UpdateMaxMana();
       }

       public void OnClickMovement()
       {
           addStatValues.AddMovementSpeed(1.5f);
           playerMovement.UpdateMovementSpeed();
       }


       public void OnClickReset()
       {
           playerStatsManager.ResetBonuses(StatTypeEnum.Haste);
           playerStatsManager.ResetBonuses(StatTypeEnum.Mastery);
           playerStatsManager.ResetBonuses(StatTypeEnum.Critical);
           playerStatsManager.ResetBonuses(StatTypeEnum.Vitality);
           playerStatsManager.ResetBonuses(StatTypeEnum.Armor);
           playerStatsManager.ResetBonuses(StatTypeEnum.Mana);
           playerStatsManager.ResetBonuses(StatTypeEnum.MovementSpeed);
           playerMana.UpdateMaxMana();
           playerHealth.UpdateMaxHealth(); 
           playerMovement.UpdateMovementSpeed();
       }

        
    }
}
