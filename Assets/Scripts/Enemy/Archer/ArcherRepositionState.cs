using Enemy.Mutant;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class ArcherRepositionState : EnemyState
    {
        private NavMeshAgent _agent;
        private Vector3 _targetPosition;
        private const float RepositionRadius = 7f; // Reposition Radius
        private const float MinRepositionDistance = 3f; // Minimum distance to doesn't go archer that location;
        private float _repositionTimer;
        private const float MaxRepositionTime = 2f; // If there is no path found during this time Archer attacks;
        private float _lastPathRecalculationTime;
        private const float PathRecalculationInterval = 0.5f; 
        private bool _hasValidPath = false;
        
    
        public ArcherRepositionState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _agent = stateMachine.GetComponent<NavMeshAgent>();
            _agent.updatePosition = false;
            _agent.updateRotation = false;
        }

        public override void Enter()
        {
            animations.EnemyRunning(true);
            _repositionTimer = 0f;
            _lastPathRecalculationTime = -PathRecalculationInterval; 
            _hasValidPath = false;
           

            if (_agent == null)
            {
                stateMachine.TransitionToState(new ArcherAttackState(stateMachine));
                return;
            }

            _agent.isStopped = false;
            
            if (!FindValidRepositionTarget())
            {
                stateMachine.TransitionToState(new ArcherAttackState(stateMachine));
            }
        }
        private bool FindValidRepositionTarget()
        {
            var awayFromPlayer = (stateMachine.transform.position - player.position).normalized;

            for (var attempts = 0; attempts < 20; attempts++)
            {
                var distance = Random.Range(MinRepositionDistance, RepositionRadius);
                var offset = awayFromPlayer * distance;
                var potentialPosition = stateMachine.transform.position + offset;

                if (NavMesh.SamplePosition(potentialPosition, out var hit, RepositionRadius, NavMesh.AllAreas))
                {
                    if (Vector3.Distance(hit.position, stateMachine.transform.position) >= MinRepositionDistance)
                    {
                        _targetPosition = hit.position;
                        _agent.SetDestination(_targetPosition);
                        _hasValidPath = true;
                        return true;
                    }
                }
            }
            return false;
        }
        
        // private bool FindValidRepositionTarget()
        // {
        //     for (var attempts = 0; attempts < 20; attempts++)
        //     {
        //         var randomDirection = Random.insideUnitSphere * RepositionRadius;
        //         randomDirection.y = 0;
        //         randomDirection += stateMachine.transform.position;
        //        
        //         if (NavMesh.SamplePosition(randomDirection, out var hit, RepositionRadius, NavMesh.AllAreas))
        //         {
        //             if (Vector3.Distance(hit.position, stateMachine.transform.position) >= MinRepositionDistance)
        //             {
        //                 _targetPosition = hit.position;
        //                 _agent.SetDestination(_targetPosition);
        //                 _hasValidPath = true;
        //                 
        //                 //Debug.DrawLine(stateMachine.transform.position, _targetPosition, Color.blue, 2f);
        //                 return true;
        //             }
        //         }
        //     }
        //     return false;
        // }
        
        public override void FixedFrameTick()
        {
            if (_agent == null) return;
            
            _repositionTimer += Time.fixedDeltaTime;
            
            if (_repositionTimer >= MaxRepositionTime)
            {
                stateMachine.TransitionToState(new ArcherAttackState(stateMachine));
                return;
            }
            
            _agent.nextPosition = stateMachine.transform.position;
            
            if (_agent.hasPath && _hasValidPath)
            {
                var desiredVelocity = _agent.desiredVelocity;
                desiredVelocity.y = 0;
                
                if (desiredVelocity.magnitude > 0.2f) 
                {
                    var move = desiredVelocity.normalized * stateMachine.MovementSpeed;
                    rigidbody.linearVelocity = new Vector3(move.x, rigidbody.linearVelocity.y, move.z);
                    
                    var lookRotation = Quaternion.LookRotation(move);
                    
                    var angleDiff = Quaternion.Angle(stateMachine.transform.rotation, lookRotation);
                    
                    if (angleDiff < 5f)
                    {
                        stateMachine.transform.rotation = lookRotation;
                    }
                    else
                    {
                        stateMachine.transform.rotation = Quaternion.Slerp(
                            stateMachine.transform.rotation, lookRotation, 
                            Time.fixedDeltaTime * stateMachine.RotationSpeed);
                    }
                
                    Debug.DrawRay(stateMachine.transform.position, move, Color.green, 0.1f);
                }
                else
                {
                    HandlePathFailure();
                }
            }
            else
            {
               HandlePathFailure();
            }
            
            if (!_agent.pathPending && 
                (_agent.remainingDistance <= _agent.stoppingDistance || 
                 !_agent.hasPath || 
                 _agent.pathStatus == NavMeshPathStatus.PathInvalid))
            {
                stateMachine.TransitionToState(new ArcherAttackState(stateMachine));
                return;
            }
            stateMachine.AlignToGround();
        }

        private void HandlePathFailure()
        {
            rigidbody.linearVelocity = new Vector3(0, rigidbody.linearVelocity.y, 0);
            
            if (Time.time - _lastPathRecalculationTime >= PathRecalculationInterval)
            {
                if (!FindValidRepositionTarget())
                {
                    stateMachine.TransitionToState(new ArcherAttackState(stateMachine));
                    return;
                }
                _lastPathRecalculationTime = Time.time;
            }
        }
    
        public override void Exit()
        {
            animations.EnemyRunning(false);
            _agent.isStopped = true;
        }
    }
}