using Enemy.Mutant;
using UnityEngine;

namespace Enemy.Archer
{
    public class ArcherAttackState : EnemyState
    {
        
        private readonly ArcherStateMachine _archerStateMachine;
        private readonly Transform _playerRangeHitPoint;
        private int _shotsFired;
        private const int MaxShotsBeforeReposition = 2;
        private float _cooldownTimer;
        private bool _attackStarted;
        private float legKickTimer;
        
        public ArcherAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _archerStateMachine  = stateMachine as ArcherStateMachine;
           _playerRangeHitPoint = _archerStateMachine?.PlayerRangeHitPoint;
        }

        public override void Enter()
        {
            animations.EnemyRunning(false);
            rigidbody.linearVelocity = new Vector3(0, rigidbody.linearVelocity.y, 0);
            _shotsFired = 0;
            legKickTimer = 0f;
            _cooldownTimer = stateMachine.AttackCooldown;
            _attackStarted = false;
          
            
            var events = stateMachine.GetComponent<ArcherAnimationEvents>();
            if (events != null)
            {
                events.Init(this);
            }

            TryAttack();
        }

        private void TryAttack()
        {
            var distanceToPlayer = Vector3.Distance(stateMachine.transform.position, player.position);

            if (distanceToPlayer <= _archerStateMachine.MeleeAttackRange)
            {
                if (!_attackStarted && legKickTimer <= 0f)
                {
                    _attackStarted = true;
                    animations.KickLeg();
                    legKickTimer = _archerStateMachine.LegKickCooldown;
                    
                }
                else
                {
                    stateMachine.TransitionToState(new ArcherRepositionState(stateMachine));
                }
            }
            else if (distanceToPlayer <= stateMachine.AttackRange)
            {
                _attackStarted = true;
                animations.EnemyShoot(); 
            }
            else
            {
                stateMachine.TransitionToState(new ArcherChaseState(stateMachine));
            }
        }

        public void OnShootAnimationEnd()
        {
            _attackStarted = false;
            if (_shotsFired < MaxShotsBeforeReposition)
            {
                _cooldownTimer = stateMachine.AttackCooldown;
            }
            else
            {
                stateMachine.TransitionToState(new ArcherRepositionState(stateMachine));
            }
        }
        
        public void LegKickDamage()
        {
            var center = stateMachine.transform.position;
            var hitCount = Physics.OverlapSphereNonAlloc(center, _archerStateMachine.CapsuleRadius,
                _archerStateMachine.Results, _archerStateMachine.PlayerLayer);

            for (var i = 0; i < hitCount; i++)
            {
                var hitCollider = _archerStateMachine.Results[i];

                if (hitCollider.TryGetComponent<IPlayerDamageable>(out var playerDamageable))
                {
                    playerDamageable?.TakeDamage(_archerStateMachine.LegKickDamage);
                    
                }
                
            }
            
            _attackStarted = false;
            _cooldownTimer = stateMachine.AttackCooldown;
          
        }
        
        public override void FixedFrameTick()
        {
            RotateTowardPlayer();
           
            if (!_attackStarted)
            {
                _cooldownTimer -= Time.deltaTime;

                if (_cooldownTimer <= 0f)
                {
                    TryAttack();
                }
            }
            if (legKickTimer > 0f)
            {
                legKickTimer -= Time.deltaTime;
            }
        }

        public void ShootArrow()
        {
            if (_archerStateMachine.ArrowSpawnPoint != null && _archerStateMachine.ArrowPrefab != null)
            {
                var arrowObject = Object.Instantiate(_archerStateMachine.ArrowPrefab, 
                    _archerStateMachine.ArrowSpawnPoint.position, _archerStateMachine.ArrowSpawnPoint.rotation);
                arrowObject.transform.Rotate(90, 0, 0);
                
                var arrowRb = arrowObject.GetComponent<Rigidbody>();

                if (arrowRb != null)
                { 
                    var shootDirection = (_playerRangeHitPoint.position - _archerStateMachine.ArrowSpawnPoint.position)
                        .normalized;
                    arrowRb.linearVelocity = shootDirection * _archerStateMachine.ArrowSpeed;
                    Object.Destroy(arrowObject, _archerStateMachine.ArrowLifetime);
                }
                _shotsFired++;
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
        public float GetKickRadius() =>_archerStateMachine.CapsuleRadius;
        public override void Exit(){}
    }
}


