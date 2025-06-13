using System;
using Enemy.Archer;
using Room_Generation;
using TMPro;
using UnityEngine;

namespace Enemy.Warlock_Boss
{
    public class WarlockHealth : MonoBehaviour, IEnemyDamageable
    {
        
        [SerializeField] private float warlockHealth;
        [SerializeField] private TextMeshProUGUI  warlockHealthText;
        
        private WarlockStateMachine _warlockStateMachine;

        private void Awake()
        {
            _warlockStateMachine = GetComponent<WarlockStateMachine>();
        }


        private void Update()
        {
           // warlockHealthText.text = $"Warlock Health: {warlockHealth}";
        }

        public void TakeDamage(float amount)
        {
            warlockHealth -= amount;

            if (warlockHealth <= 0)
            { 
                _warlockStateMachine.TransitionToState(new WarlockDeath(_warlockStateMachine));
                EnemyTracker.Instance?.RegisterEnemyDeath();
            }
           
        }
    }
}
