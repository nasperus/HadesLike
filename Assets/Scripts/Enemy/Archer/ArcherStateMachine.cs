using System;
using Enemy.Mutant;
using Room_Generation;
using UnityEngine;

namespace Enemy.Archer
{
    public class ArcherStateMachine : EnemyStateMachine
    {
        [field: SerializeField] public Transform ArrowSpawnPoint {get; private set;}
        [field: SerializeField] public GameObject ArrowPrefab {get; private set;}
        [field: SerializeField] public float ArrowSpeed {get; private set;}
        [field: SerializeField] public float ArrowLifetime {get; private set;}
        [field: SerializeField] public Transform PlayerRangeHitPoint {get; private set;}
        [field: SerializeField] public float MeleeAttackRange { get; private set; }
        [field: SerializeField] public int LegKickDamage { get; private set; }
        [field: SerializeField] public LayerMask PlayerLayer { get; private set; }
        [field: SerializeField] public float CapsuleRadius { get; private set; }
        [field: SerializeField] public float LegKickCooldown { get; private set; }

        private EnemyState _previousState;
      
        public Collider[] Results { get; private set; } = new Collider[5];

        private void Awake()
        {
            AttackCooldown -= RoomManager.IncreaseAttackSpeed;
            ArrowSpeed += RoomManager.IncreaseArrowSpeed;
        }

        private void Start()
        {
            TransitionToState(new ArcherChaseState(this));
        }
        public void SetPlayerRangeHitPoint(Transform hitPoint)
        {
            PlayerRangeHitPoint = hitPoint;
        }

        public override void TakeDamage()
        {
            if (CurrentState is ArcherHitState) return;
            _previousState = CurrentState;
            TransitionToState(new ArcherHitState(this, _previousState));
        }
        
    
    }
}
