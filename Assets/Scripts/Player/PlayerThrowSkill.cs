using DG.Tweening;
using Player.Skills;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerThrowSkill : PlayerAbilityBase
    {
        [SerializeField] private StatCollection statCollection;
        [SerializeField] private PlayerActionSate playerActionSate;
        [SerializeField] private Transform javelinPosition;
        [SerializeField] private GameObject javelinPrefab;
        [SerializeField] private float javelinSpeed;
        [SerializeField] private float attackCooldown;
        private float _attackCd;
        private float _movementFreezeTimer;
        private const float BaseMovementFreezeTime = 0.8f;


        private void Update()
        {
            AttackCooldown();
        }

        private void AttackCooldown()
        {
           
            if (_attackCd > 0)
            {
                _attackCd -= Time.deltaTime;
            }
            if (IsMovementFrozen)
            {
                _movementFreezeTimer -= Time.deltaTime;
                if (_movementFreezeTimer <= 0f)
                {
                    IsMovementFrozen = false;
                }
            }
        }
        
        private void OnThrow(InputValue value)
        {
            if (!value.isPressed || _attackCd > 0f) return;
            playerActionSate.StartAttack();
            _attackCd = ApplyStatsToAbilities.ApplyHasteSpeed(attackCooldown, statCollection);
            CalculateMouseRay();
            SetupRayDirection();
            DashForward();
            RotatePlayer();
            _movementFreezeTimer = ApplyStatsToAbilities.ApplyHasteSpeed(BaseMovementFreezeTime, statCollection);
            IsMovementFrozen = true;
            UpdateJavelinThrowSpeed();
        }
        
        private void UpdateJavelinThrowSpeed()
        {
            var animationSpeed = ApplyStatsToAbilities.ApplyHasteCastAndAttackSpeed(statCollection);
            playerAnimations.JavelinThrow(animationSpeed);
        }

        private void DashForward()
        {
            var dashDistance = 0.5f;
            var duration = 0.15f;
            var direction = transform.forward;
            transform.DOMove(transform.position + direction * dashDistance, duration).SetEase(Ease.OutQuad);
        }
        
        public void InstantiateJavelin()
        {
            var javelinObj =  Instantiate(javelinPrefab, javelinPosition.position, Quaternion.LookRotation(javelinPosition.forward));
            
            var javelinRigidBody = javelinObj.GetComponent<Rigidbody>();
           
            javelinObj.transform.Rotate(Vector3.right, 90);
           
            javelinRigidBody.linearVelocity = javelinPosition.forward * javelinSpeed;
           
        }
    }
}
