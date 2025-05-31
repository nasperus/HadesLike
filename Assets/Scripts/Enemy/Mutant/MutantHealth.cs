
using Enemy.Archer;
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
        }
        

        public void TakDamage(float amount)
        {
            if(_enemyStateMachine.IsDead) return;
            _enemyStateMachine.TakeDamage();
            
            mutantHealth -= amount;
            if (mutantHealth <= 0)
            {
                _enemyStateMachine.TransitionToState(new MutantDeathState(_enemyStateMachine));
               
               
            }
            else
            {
                //Debug.Log($"{mutantHealth}");
            }
            
        }

       
    }
}
