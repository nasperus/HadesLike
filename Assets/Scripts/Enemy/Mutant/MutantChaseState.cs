using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Mutant
{
    public class MutantChaseState : EnemyState
    {
        private NavMeshAgent _agent;

        public MutantChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _agent = stateMachine.GetComponent<NavMeshAgent>();
            _agent.updatePosition = false;
            _agent.updateRotation = false;
           
        }

        public override void Enter()
        {
            animations.EnemyRunning(true);
            _agent.isStopped = false;
            //Debug.Log("Entered ChaseState");
            
        }

        public override void FixedFrameTick()
        {
            if (player == null) return;
         
            
            var distance = Vector3.Distance(stateMachine.transform.position, player.position);
            
            if (distance <= stateMachine.AttackRange)
            {
                stateMachine.TransitionToState(new MutantAttackState(stateMachine));
                return;
            }
               
            _agent.SetDestination(player.position);
            _agent.nextPosition = stateMachine.transform.position;
            
            if (_agent.hasPath)
            {
                var desiredVelocity = _agent.desiredVelocity;
                desiredVelocity.y = 0;
                var move = desiredVelocity.normalized * stateMachine.MovementSpeed;
                rigidbody.linearVelocity = new Vector3(move.x, rigidbody.linearVelocity.y, move.z);
                var lookRotation = Quaternion.LookRotation(move);
                stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation, Time.fixedDeltaTime * stateMachine.RotationSpeed);
            }
            stateMachine.AlignToGround();
            
        }
        public override void Exit()
        {
            //Debug.Log("Exited ChaseState");
            animations.EnemyRunning(false);
            _agent.isStopped = true;
        }
    }
}
