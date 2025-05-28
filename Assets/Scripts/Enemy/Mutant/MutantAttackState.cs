using UnityEngine;

namespace Enemy.Mutant
{
    public class MutantAttackState : EnemyState
    {
        private float _cooldownTimer;
        
        public MutantAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            animations.EnemyRunning(false);
            rigidbody.linearVelocity = new Vector3(0, rigidbody.linearVelocity.y, 0);
            animations.MeleeAttack();
            _cooldownTimer = stateMachine.AttackCooldown;
            
        }

        public override void FixedFrameTick()
        {
            _cooldownTimer -= Time.deltaTime;
          
            RotateTowardPlayer();
            if (_cooldownTimer <= 0f)
            {
                stateMachine.TransitionToState(new MutantChaseState(stateMachine));
            }
        }
        private void RotateTowardPlayer()
        {
            if (player != null)
            {
                var direction = (player.position - stateMachine.transform.position).normalized;
                direction.y = 0; 
                if (direction != Vector3.zero)
                {
                    var lookRotation = Quaternion.LookRotation(direction);
                    stateMachine.transform.rotation = Quaternion.Slerp(
                        stateMachine.transform.rotation,
                        lookRotation,
                        Time.fixedDeltaTime * stateMachine.RotationSpeed
                    );
                }
            }
        }

        public override void Exit(){}
       
    }
}
