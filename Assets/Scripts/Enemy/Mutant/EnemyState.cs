using UnityEngine;

namespace Enemy.Mutant
{
    public abstract class EnemyState
    {
        protected EnemyStateMachine stateMachine;
        protected Transform player;
        protected EnemyAnimations animations;
        protected Rigidbody rigidbody;
        

        protected EnemyState(EnemyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            this.player = stateMachine.Player;
            this.animations = stateMachine.Animations;
            this.rigidbody = stateMachine.Rigidbody;
          
        }
        
        public virtual void Enter() {}
        public virtual void Exit() {}
        public virtual void FrameTick() {}
        public virtual void FixedFrameTick() {}

    }
}
