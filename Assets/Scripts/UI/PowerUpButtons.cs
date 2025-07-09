using System;
using System.Collections.Generic;
using Player;
using Player.Skills;
using Prayers;
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

        private StrikeLightning _strikeLightning;
        private PlayerDebuff _playerDebuff;
        private AoeFireDamage _aoeFireDamage;
        private PlayerLeftClickAttack _playerLeftClick;
        private PlayerThrowSkill _playerThrowSkill;





        public void SetPlayerReferences(GameObject player)
        {
            _addStatValues = player.GetComponent<AddingStatValues>();
            _playerStatsManager = player.GetComponent<PlayerStatsManager>();
            _playerHealth = player.GetComponent<PlayerHealth>();
            _playerArmor = player.GetComponent<PlayerArmor>();
            _playerMana = player.GetComponent<PlayerMana>();
            _playerMovement = player.GetComponent<PlayerMovement>();
            _statCollection = player.GetComponent<StatCollection>();
            _strikeLightning = player.GetComponent<StrikeLightning>();
            _playerDebuff = player.GetComponent<PlayerDebuff>();
            _aoeFireDamage = player.GetComponent<AoeFireDamage>();
            _playerLeftClick = player.GetComponent<PlayerLeftClickAttack>();
            _playerThrowSkill = player.GetComponent<PlayerThrowSkill>();
            InitializePowerUpActions();

        }

        private Dictionary<PowerUpType, Action> _powerUpActions;
        public static HashSet<PowerUpType> UnlockedSkills = new();

        private void InitializePowerUpActions()
        {
            _powerUpActions = new Dictionary<PowerUpType, Action>
            {
                { PowerUpType.Haste, () => _addStatValues.AddHaste(2f) },
                { PowerUpType.Mastery, () => _addStatValues.AddMastery(5f) },
                { PowerUpType.Critical, () => _addStatValues.AddCritical(3f) },
                {
                    PowerUpType.Vitality, () =>
                    {
                        _addStatValues.AddVitality(15f);
                        _playerHealth.UpdateMaxHealth();
                    }
                },
                { PowerUpType.Armor, () => _addStatValues.AddArmor(5f) },
                {
                    PowerUpType.Mana, () =>
                    {
                        _addStatValues.AddMana(10f);
                        _playerMana.UpdateMaxMana();
                    }
                },
                {
                    PowerUpType.MovementSpeed, () =>
                    {
                        _addStatValues.AddMovementSpeed(1.5f);
                        _playerMovement.UpdateMovementSpeed();
                    }
                },
                { PowerUpType.LightningDamage, () => _strikeLightning.IncreaseLightningStrikeDamage(0.2f)},
                { PowerUpType.LightningRadius, () => _strikeLightning.IncreaseLightningStrikeRadius(0.4f)},
                { PowerUpType.DotDamage, () => _playerDebuff.IncreaseDotDamage(0.4f)},
                { PowerUpType.AoeDamage, () => _aoeFireDamage.IncreaseAoeDamage(0.2f)},
                { PowerUpType.ActivateChainLightning, () => _strikeLightning.ActivateChainLightning()},
                { PowerUpType.ActivateAoeDot, () => _playerDebuff.ActivateChainDot()},
                { PowerUpType.LifeSteal, () => LifeSteal.IsEnabled = true},
                { PowerUpType.Evasion, () => CriticalMiss.CanMiss = true},
                { PowerUpType.AutoAttack, () => _playerLeftClick.IncreaseAutoAttackDamage(0.2f)},
                { PowerUpType.ActivateLightningStrike, () => _strikeLightning.ActivateLightningStrike()},
                { PowerUpType.ActivateDot, () => _playerDebuff.ActivateDot()},
                { PowerUpType.ActivateFire, () => _aoeFireDamage.ActivateAoeFire()},
                { PowerUpType.ThrowJavelin, () => _playerThrowSkill.IncreaseBaseJavelinDamage(0.2f)},
                {PowerUpType.ActivateJavelinThrow, () => _playerThrowSkill.ActivateJavelinThrow()}
            };
        }
        public void ApplyPowerUp(PowerUpType type)
        {
            if (_powerUpActions.TryGetValue(type, out var action))
            {
                action?.Invoke();
            }
            if (type is PowerUpType.ActivateLightningStrike or PowerUpType.ActivateDot ||
                type == PowerUpType.ActivateFire ||
                type == PowerUpType.ActivateChainLightning ||  
                type == PowerUpType.ActivateAoeDot)
            {
                UnlockedSkills.Add(type);
            }
        }


    }
}



