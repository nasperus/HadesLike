
using Enemy.Archer;
using Room_Generation;
using UnityEngine;

namespace Enemy.Mutant
{
    public class MutantHealth : MonoBehaviour, IEnemyDamageable
    {
        [SerializeField] private float mutantHealth;
        private EnemyStateMachine _enemyStateMachine;

        private void Awake()
        {
            _enemyStateMachine = GetComponent<EnemyStateMachine>();
            mutantHealth += RoomManager.IncreaseEnemyHealth;
        }
        

        public void TakeDamage(float amount)
        {
            if(_enemyStateMachine.IsDead) return;
            _enemyStateMachine.TakeDamage();
            
            mutantHealth -= amount;
            if (mutantHealth <= 0)
            { 
                _enemyStateMachine.TransitionToState(new MutantDeathState(_enemyStateMachine));
                EnemyTracker.Instance?.RegisterEnemyDeath();
               
            }
            else
            {
                //Debug.Log($"{mutantHealth}");
            }
            
        }

       
    }
}
