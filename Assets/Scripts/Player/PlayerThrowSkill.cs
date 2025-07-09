using System;
using Ability_System.Core_Base_Classes;
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
        [SerializeField] private float baseJavelinDamage;

       
        
        private float _attackCd;
        private float _movementFreezeTimer;
        private const float BaseMovementFreezeTime = 0.8f;
        private bool _javelinThrow = false;

        private void Awake()
        {
            InitializeAbility();
        }


        private void Update()
        {
            AttackCooldown();
        }

        private void InitializeAbility()
        {
            var modifier = AbilityFactory.JavelinDamage(baseJavelinDamage, 0);
            Init(modifier);
        }

        public void IncreaseBaseJavelinDamage(float multiplier)
        {
            Stats?.IncreaseStats(StatType.Damage,multiplier);
        }

        public void ActivateJavelinThrow() => _javelinThrow = true;
        
        
        private void OnThrow(InputValue value)
        {
            if (!_javelinThrow) return;
            if (!value.isPressed || _attackCd > 0f) return;
            playerActionSate.StartAttack();
            _attackCd = ApplyStatsToAbilities.ApplyHasteSpeed(attackCooldown, statCollection);
            CalculateMouseRay();
            SetupRayDirection();
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

      
        public void InstantiateJavelin()
        {
            var javelinObj =  Instantiate(javelinPrefab, javelinPosition.position, Quaternion.LookRotation(ShootDirection));
            
            if (javelinObj.TryGetComponent<Rigidbody>(out var javelinRigidBody))
            {
                javelinRigidBody.linearVelocity = ShootDirection * javelinSpeed;
            }
            
            if (javelinObj.TryGetComponent<JavelinThrow>(out var javelinThrow))
            {
                var finalDamage = GetFinalDamage();
                var masteryApplied = ApplyStatsToAbilities.ApplyMastery(finalDamage, statCollection);
                var critApplied = ApplyStatsToAbilities.ApplyCritChance(masteryApplied, statCollection); 
                javelinThrow.SetBaseDamage(critApplied);
            }
            javelinObj.transform.Rotate(Vector3.right, 90);
            
           
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
    }
}
