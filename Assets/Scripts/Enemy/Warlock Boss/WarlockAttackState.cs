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
                animations.MeteorIce();
                _hasAttacked = true;    
            }
            else
            {
                stateMachine.TransitionToState(new WarlockMoveState(stateMachine));
            }
            _cooldownTimer = stateMachine.AttackCooldown;
        }

        public override void FixedFrameTick()
        {
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
