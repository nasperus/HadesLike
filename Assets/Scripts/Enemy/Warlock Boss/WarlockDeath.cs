using Enemy.Mutant;
using UnityEngine;

namespace Enemy.Warlock_Boss
{
    public class WarlockDeath : EnemyState
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public WarlockDeath(EnemyStateMachine stateMachine) : base(stateMachine)
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
