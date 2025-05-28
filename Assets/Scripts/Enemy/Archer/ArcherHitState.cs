using Enemy.Mutant;
using UnityEngine;

namespace Enemy.Archer
{
    public class ArcherHitState : EnemyState
    {
        
        private float _hitDuration = 0.5f;
        private float _timer;
        private EnemyState _currentState;
        public ArcherHitState(EnemyStateMachine stateMachine, EnemyState currentState) : base(stateMachine)
        {
            _currentState =  currentState;
        }

        public override void Enter()
        {
            _timer = _hitDuration;
            animations.EnemyHit();
            rigidbody.linearVelocity = Vector3.zero;
        }

        public override void FixedFrameTick()
        {
            _timer -= Time.fixedDeltaTime;
            if (_timer <= 0)
            {
                stateMachine.TransitionToState(_currentState);
            }
            
        }
        public override void Exit() { }
    }
}
