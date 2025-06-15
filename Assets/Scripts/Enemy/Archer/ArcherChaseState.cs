using Enemy.Mutant;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class ArcherChaseState : EnemyState
    {
        private readonly NavMeshAgent _agent;
     
        public ArcherChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _agent = stateMachine.GetComponent<NavMeshAgent>();
            _agent.updatePosition = false;
            _agent.updateRotation = false;
        }

        public override void Enter()
        {
            animations.EnemyRunning(true);
            _agent.isStopped = false;
        }
        
        public override void FixedFrameTick()
        {
            if (player == null) return;
            
            var distance = Vector3.Distance(stateMachine.transform.position, player.position);
            
            if (distance <= stateMachine.AttackRange && HasLineOfSightToPlayer())
            {
                stateMachine.TransitionToState(new ArcherAttackState(stateMachine));
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

        private bool HasLineOfSightToPlayer()
        {
            var origin = stateMachine.transform.position + Vector3.up * 1.2f; 
            var direction = (player.position - origin).normalized;
            var distance = Vector3.Distance(origin, player.position);

            if (Physics.Raycast(origin, direction, out var hit, distance))
            {
                return hit.collider.CompareTag("Player");
            }

            return false;
        }
        
        public override void Exit()
        {
            _agent.isStopped = true;
            animations.EnemyRunning(false);
        }
       
    }
}
