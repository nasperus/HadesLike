using Enemy.Mutant;
using UnityEngine;

namespace Enemy.Archer
{
    public class ArcherDeathState : EnemyState
    {
        
        public ArcherDeathState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }


        public override void Enter()
        {
            stateMachine.SetDead();
            animations.EnemyDeath();
            stateMachine.StartCoroutine(DestroyAfterDeath(1.9f));
        }

        public override void FixedFrameTick()
        {
            
        }

        private System.Collections.IEnumerator DestroyAfterDeath(float time)
        {
            yield return new WaitForSeconds(time);
            Object.Destroy(stateMachine.gameObject);
        }
        
        public override void Exit() {}
    }
}
