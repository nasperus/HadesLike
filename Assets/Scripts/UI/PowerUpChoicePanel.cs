using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace UI
{
   
    public class PowerUpChoicePanel : MonoBehaviour
    {
        [SerializeField] private GameObject powerUpButtonPrefab;
        [SerializeField] private Transform buttonParent;
        [SerializeField] private PowerUpButtons powerUpButtons;

        private readonly List<PowerUpType> _allPowerUps = new()
        {
            PowerUpType.Haste,
            PowerUpType.Mastery,
            PowerUpType.Critical,
            PowerUpType.Vitality,
            PowerUpType.Armor,
            PowerUpType.Mana,
            PowerUpType.MovementSpeed,
            PowerUpType.LightningDamage,
            PowerUpType.LightningRadius,
            PowerUpType.DotDamage,
            PowerUpType.AoeDamage,
            PowerUpType.ActivateChainLightning,
            PowerUpType.ActivateAoeDot,
            PowerUpType.LifeSteal,
            PowerUpType.Evasion,
            PowerUpType.AutoAttack,
            PowerUpType.ActivateLightningStrike,
            PowerUpType.ActivateDot,
            PowerUpType.ActivateFire
            
        };
        
        private readonly HashSet<PowerUpType> _oneTimePowerUps = new()
        {
            PowerUpType.ActivateChainLightning,
            PowerUpType.ActivateAoeDot,
            PowerUpType.LifeSteal,
            PowerUpType.Evasion,
            PowerUpType.ActivateLightningStrike,
            PowerUpType.ActivateDot,
            PowerUpType.ActivateFire
            
        };
        
       
        private readonly Dictionary<PowerUpType, PowerUpType> _amplifierDependencies = new()
        {
            { PowerUpType.LightningDamage, PowerUpType.ActivateLightningStrike },
            { PowerUpType.LightningRadius, PowerUpType.ActivateLightningStrike },
            { PowerUpType.ActivateChainLightning, PowerUpType.ActivateLightningStrike },

            { PowerUpType.DotDamage, PowerUpType.ActivateDot },
            { PowerUpType.ActivateAoeDot, PowerUpType.ActivateDot },
            
            { PowerUpType.AoeDamage, PowerUpType.ActivateFire },
            
        };

        public void ShowRandomPowerUps(int amount )
        {
            ClearOldButtons();
            StartCoroutine(DelayedPauseAndShow(amount));
        }
        private IEnumerator DelayedPauseAndShow(int amount)
        {
            yield return new WaitForSecondsRealtime(1f);

            Time.timeScale = 0f;

            List<PowerUpType> chosen = GetRandomPowerUps(amount);

            const float verticalSpacing = -200f;
            const float startY = 400f;

            for (var i = 0; i < chosen.Count; i++)
            {
                var type = chosen[i];

                var buttonGo = Instantiate(powerUpButtonPrefab, buttonParent);
                buttonGo.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, startY + i * verticalSpacing);

                var label = buttonGo.GetComponentInChildren<TMP_Text>();
                label.text = GetPowerUpText(type);

                var btn = buttonGo.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    powerUpButtons.ApplyPowerUp(type);
                    
                    if (_oneTimePowerUps.Contains(type))
                    {
                        _allPowerUps.Remove(type);
                    }
                    ClearOldButtons();
                    Time.timeScale = 1f; 
                });
            }
        }
        
        private void ClearOldButtons()
        {
            foreach (Transform child in buttonParent)
                Destroy(child.gameObject);
        }
        

        private List<PowerUpType> GetRandomPowerUps(int amount)
        {
            List<PowerUpType> validChoices = new();

            foreach (var powerUp in _allPowerUps)
            {
                if (_amplifierDependencies.TryGetValue(powerUp, out var requiredSkill))
                {
                    if (!PowerUpButtons.UnlockedSkills.Contains(requiredSkill))
                        continue;
                }
                validChoices.Add(powerUp);
            }
            
            for (var i = 0; i < validChoices.Count; i++)
            {
                var temp = validChoices[i];
                var rand = Random.Range(i, validChoices.Count);
                validChoices[i] = validChoices[rand];
                validChoices[rand] = temp;
            }
            return validChoices.GetRange(0, Mathf.Min(amount, validChoices.Count));
        }
        private readonly Dictionary<PowerUpType, string> _powerUpTexts = new()
        {
            { PowerUpType.Haste, "+2 Haste" },
            { PowerUpType.Mastery, "+5 Mastery" },
            { PowerUpType.Critical, "+3 Critical" },
            { PowerUpType.Vitality, "+15 HP" },
            { PowerUpType.Armor, "+5 Armor" },
            { PowerUpType.Mana, "+10 Mana" },
            { PowerUpType.MovementSpeed, "+1.5 Movement" },
            { PowerUpType.LightningDamage, "+20% Lightning Damage" },
            { PowerUpType.LightningRadius, "+40% Lightning Radius" },
            { PowerUpType.DotDamage, "+40% DoT Damage" },
            { PowerUpType.AoeDamage, "+20% AoE Fire Damage" },
            { PowerUpType.ActivateChainLightning, "Lightning Strike Hit Nearby Enemies" },
            { PowerUpType.ActivateAoeDot, "Dot Now Spread Nearby Enemies" },
            { PowerUpType.LifeSteal, "LifeSteal (Higher Damage Higher Value)" },
            { PowerUpType.Evasion, "+20% Evasion" },
            { PowerUpType.AutoAttack, "+20% AutoAttack Damage" },
            { PowerUpType.ActivateLightningStrike, "Activate Lightning Strike" },
            { PowerUpType.ActivateDot, "Activate Debuff Damage" },
            { PowerUpType.ActivateFire, "Activate AOE Fire Damage" },
        };

        private string GetPowerUpText(PowerUpType type)
        {
            return _powerUpTexts.GetValueOrDefault(type);
        }
    }
}
