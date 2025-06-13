using Enemy.Mutant;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Warlock_Boss
{
    public class WarlockMoveState : EnemyState
    {
        private readonly NavMeshAgent _navMeshAgent;
        public WarlockMoveState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updatePosition = false;
        }

        public override void Enter()
        {
            animations.EnemyRunning(true);
            _navMeshAgent.isStopped = false;
        }
        

        public override void FixedFrameTick()
        {
            if (player == null) return;
            
            var distance = Vector3.Distance(stateMachine.transform.position, player.position);

            if (distance <= stateMachine.AttackRange)
            {
                stateMachine.TransitionToState(new WarlockAttackState(stateMachine));
                return;
            }
            _navMeshAgent.SetDestination(player.position);
            _navMeshAgent.nextPosition = stateMachine.transform.position;

            if (_navMeshAgent.hasPath)
            {
                var desiredVelocity = _navMeshAgent.desiredVelocity;
                desiredVelocity.y = 0;
                var move = desiredVelocity.normalized * stateMachine.MovementSpeed;
                rigidbody.linearVelocity = new Vector3(move.x, rigidbody.linearVelocity.y, move.z);
                var lookRotation  = Quaternion.LookRotation(move);
                stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation,
                    Time.fixedDeltaTime * stateMachine.RotationSpeed);
            }
            stateMachine.AlignToGround();
        }
        
        public override void Exit()
        {
            _navMeshAgent.isStopped = true;
            animations.EnemyRunning(false);
        }
        
    }
}
