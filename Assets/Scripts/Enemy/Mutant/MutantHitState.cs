using UnityEngine;

namespace Enemy.Mutant
{
    public class MutantHitState : EnemyState
    {
        private float _hitDuration = 0.5f;
        private float _timer;
        private EnemyState _currentState;
        public MutantHitState(EnemyStateMachine stateMachine, EnemyState currentState) : base(stateMachine)
        {
            _currentState = currentState;
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
            if (_timer <= 0f)
            {
                stateMachine.TransitionToState(_currentState);
            }
        }
    }
}
