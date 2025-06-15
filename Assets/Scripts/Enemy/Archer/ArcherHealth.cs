using System;
using Room_Generation;
using TMPro;
using UnityEngine;

namespace Enemy.Archer
{
    public class ArcherHealth : MonoBehaviour, IEnemyDamageable
    {
        [SerializeField] private float archerHealth;
        [SerializeField] private TextMeshProUGUI archerHealthText;
        private ArcherStateMachine _archerStateMachine;
      

        private void Awake()
        {
            _archerStateMachine = GetComponent<ArcherStateMachine>();
            archerHealth += RoomManager.IncreaseEnemyHealth;
        }

        // private void Update()
        // {
        //     archerHealthText.text = $"Archer Health: {archerHealth}";
        // }

        public void TakeDamage(float amount)
        {
            if (_archerStateMachine.IsDead) return;
            _archerStateMachine.TakeDamage();
            archerHealth -= amount;
            if (archerHealth <= 0)
            {
               _archerStateMachine.TransitionToState( new ArcherDeathState(_archerStateMachine));
               EnemyTracker.Instance?.RegisterEnemyDeath();
            }
            else
            {
               // Debug.Log($"{archerHealth}");
            }
            
        }
        
    }
}
