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
        
        public float CurrentMana
        {
            get => currentMana;
            set => currentMana = value;
        }
        public float MaxMana => maxMana;

        private void Start()
        {
            currentMana = maxMana;
        }
        
        public void UpdateMaxMana()
        {
            maxMana = playerStatsManager.GetStatValue("Mana");
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        }
    }
}
