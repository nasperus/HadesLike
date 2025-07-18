using System;
using Room_Generation;
using UnityEngine;

namespace Enemy.Mutant
{
    public class EnemyStateMachine : MonoBehaviour
    {
        [field: SerializeField] public Transform Player { get; private set; }
        [field: SerializeField] public EnemyAnimations Animations { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; set; }
        [field: SerializeField] public LayerMask GroundMask { get; private set; }
        
        private EnemyState _currentState;
        private EnemyState _previousState;
        protected EnemyState CurrentState => _currentState;
        private bool _isDead = false;
        public bool IsDead => _isDead;
        

        private void Awake()
        {
            AttackCooldown -= RoomManager.IncreaseAttackSpeed;
        }

        private void Start()
        {
            TransitionToState(new MutantChaseState(this));
        }

        public Transform GetPlayerTransform(Transform player)
        {
           return Player = player;
        }

        private void Update()
        {
            _currentState?.FrameTick();
        }

        private void FixedUpdate()
        {
            _currentState?.FixedFrameTick();
        }

        public void SetDead()
        {
            _isDead = true;
        }

        public void TransitionToState(EnemyState newState)
        {
            if (_isDead) return;
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }
        public virtual void TakeDamage()
        {
            if (CurrentState is MutantHitState) return;
            _previousState = CurrentState;
            TransitionToState(new MutantHitState(this, _previousState));
        }
        
        public void AlignToGround(float raycastHeight = 1.0f, float raycastDistance = 2.0f)
        {
            var rayOrigin = transform.position + Vector3.up * raycastHeight;
            if (Physics.Raycast(rayOrigin, Vector3.down, out var hit, raycastDistance,GroundMask))
            {
                var pos = transform.position;
                pos.y = hit.point.y;
                transform.position = pos;
            }
        }
   
    }
}
