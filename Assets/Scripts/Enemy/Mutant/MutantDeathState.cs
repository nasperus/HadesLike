using UnityEngine;
namespace Enemy.Mutant
{
    public class MutantDeathState : EnemyState
    {
        public MutantDeathState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.SetDead();
            animations.EnemyDeath();
            stateMachine.StartCoroutine(DestroyAfterDeath(1.9f));
            
        }
        
        private System.Collections.IEnumerator DestroyAfterDeath(float time)
        {
            yield return new WaitForSeconds(time);
            Object.Destroy(stateMachine.gameObject);
        }
    }
}
