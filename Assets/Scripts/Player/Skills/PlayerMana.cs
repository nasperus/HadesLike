using System;
using Stats;
using TMPro;
using UnityEngine;

namespace Player.Skills
{
    public class PlayerMana : MonoBehaviour
    {
        [SerializeField] private float maxMana;
        [SerializeField] private float currentMana;
        [SerializeField] private PlayerStatsManager playerStatsManager;
        private float _originalMaxMana;
        public float CurrentMana
        {
            get => currentMana;
            set => currentMana = value;
        }
        public float MaxMana => maxMana;

        private void Start()
        {
            _originalMaxMana = maxMana;
            currentMana = maxMana;
        }
        
        public void UpdateMaxMana()
        {
            
            maxMana = ApplyStatsToAbilities.ApplyManaBonus(_originalMaxMana, playerStatsManager.GetStatCollection());
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        }
    }
}
