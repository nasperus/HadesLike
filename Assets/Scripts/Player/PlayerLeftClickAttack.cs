using System;
using Player.Skills;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerLeftClickAttack : PlayerAbilityBase
    {
        [SerializeField] private PlayerActionSate playerActionSate;
        [SerializeField] private float damage;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private StatCollection statCollection;
        [SerializeField] private float attackCooldown;

        private float _attackCd;
        private LifeSteal _lifeSteal;
        
        private float _movementFreezeTimer;
        private const float BaseMovementFreezeTime = 2f;

        private void Awake()
        {
            _lifeSteal = new LifeSteal();
        }

        private void Update()
        {
            AttackCooldownCd();
        }

        private void OnMouseClickLeft(InputValue value)
        {
            if (!value.isPressed) return;
            IsLeftClicking = true;
            if(_attackCd > 0) return;
            if (playerActionSate.IsAttacking) return;
            playerActionSate.StartAttack();
            _attackCd = ApplyStatsToAbilities.ApplyHasteSpeed(attackCooldown, statCollection);
           
           // _lifeSteal.GetLifeSteal(damage, playerHealth, statCollection);
            DamageEnemiesAroundClickedEnemy();
            CalculateMouseRay();
            SetupRayDirection();
            RotatePlayer();
            _movementFreezeTimer = ApplyStatsToAbilities.ApplyHasteSpeed(BaseMovementFreezeTime, statCollection);
            IsMovementFrozen = true;
            UpdateAnimationSpeed();


        }

        private void UpdateAnimationSpeed()
        {
            var animationSpeed = ApplyStatsToAbilities.ApplyHasteCastAndAttackSpeed(statCollection);
            playerAnimations.PlayerAutoAttack(animationSpeed);
        }

        public void TriggerAutoAttack()
        {
            Debug.Log("auto attack trigger");
        }

        private void AttackCooldownCd()
        {
            if (_attackCd > 0f)
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
        
        
    }
}