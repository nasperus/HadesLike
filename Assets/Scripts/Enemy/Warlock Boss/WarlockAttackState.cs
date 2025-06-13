using Enemy.Mutant;
using UnityEngine;

namespace Enemy.Warlock_Boss
{
    public class WarlockAttackState : EnemyState
    {

        private WarlockStateMachine _warlockStateMachine;
        private float _cooldownTimer;
        private bool _hasAttacked;
        public WarlockAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _warlockStateMachine = stateMachine as WarlockStateMachine;
        }
        
        
        public override void Enter()
        {
            animations.EnemyRunning(false);
            rigidbody.linearVelocity = Vector3.zero;
           
        }

        private void TryAttack()
        {
            if (_hasAttacked) return;
            var distance = Vector3.Distance(stateMachine.transform.position, player.position);

            if (distance <= stateMachine.AttackRange)
            {
                _warlockStateMachine.ChooseRandomSkill();
                switch (_warlockStateMachine.CurrentSkillIndex)
                {
                    case 0:
                        animations.MeteorIce(); 
                        break;
                    case 1:
                        animations.LaserBeam(); 
                        break;
                    case 2:
                        animations.RedStone(); 
                        break;
                }
                
                _hasAttacked = true;    
                _cooldownTimer = stateMachine.AttackCooldown;
            }
            else
            {
                stateMachine.TransitionToState(new WarlockMoveState(stateMachine));
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

        

        public override void FixedFrameTick()
        {
            RotateTowardPlayer();
           _cooldownTimer -= Time.deltaTime;
           if (_cooldownTimer <= 0f)
           {
               _hasAttacked = false;
               TryAttack();
           }
        }


        public override void Exit()
        {
            
        }
    }
}
