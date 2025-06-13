using System;
using Ability_System.Core_Base_Classes;
using Enemy.Archer;
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
        [SerializeField] private float damageRadius;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private StatCollection statCollection;
        [SerializeField] private float attackCooldown;
        [SerializeField] private GameObject vfxPrefab;
        [SerializeField] private Transform slashTransform;
        
        
        private float _comboMaxDelay = 2f;
        private float _lastComboTime;
        private int _comboIndex;
        
        private bool _bufferedAttackInput;
        private float _bufferTime;
        private float _bufferTimer;
        
        private float _attackCd;
        private LifeSteal _lifeSteal;
        
        private float _movementFreezeTimer;
        private const float BaseMovementFreezeTime = 1f;

        private void Awake()
        {
            _lifeSteal = new LifeSteal();
            InitializeAbilities();
        }

        private void Update()
        {
            AttackCooldownCd();
            
        }

        public void ResetComboIndex()
        {
            _comboIndex = 0;
        }

        private void OnMouseClickLeft(InputValue value)
        {
            if (!value.isPressed) return;
            if (_attackCd > 0 || playerActionSate.IsAttacking)
            {
                if (playerActionSate.IsAttacking) 
                {
                    _bufferedAttackInput = true;
                    _bufferTimer = _bufferTime;
                }
                return;
            }

            TriggerAutoAttack(); 
        }


        

        private void ComboChainAttack()
        {
            if (Time.time - _lastComboTime > _comboMaxDelay)
            {
                _comboIndex = 0;
            }
            _comboIndex++;
            if (_comboIndex > 3) _comboIndex = 1;
            _lastComboTime = Time.time;

        }

        private void InitializeAbilities()
        {
            var modifier = AbilityFactory.AutoAttack(damage, damageRadius);
            Init(modifier);
        }

      
        private void VfxPrefabSpawner()
        {
            var originalRotation = slashTransform.rotation;
            var rotated = originalRotation * Quaternion.Euler(0, -45f, 0);
            var spawnSlash = Instantiate(vfxPrefab, slashTransform.position,rotated, slashTransform);
            Destroy(spawnSlash, 1f);
        }

        private void UpdateAnimationSpeed()
        {
            var animationSpeed = ApplyStatsToAbilities.ApplyHasteCastAndAttackSpeed(statCollection);
            playerAnimations.TriggerComboAttack(animationSpeed,_comboIndex);
        }
        
        public void AnimationEvent_MidAttackEffect()
        {
            DamageEnemiesAroundClickedEnemy();
            //_lifeSteal.GetLifeSteal(damage, playerHealth, statCollection); 
            VfxPrefabSpawner();
            DealDamage();
        }

        private void TriggerAutoAttack()
        {
            playerActionSate.StartAttack();
            _attackCd = ApplyStatsToAbilities.ApplyHasteSpeed(attackCooldown, statCollection);
            ComboChainAttack();
            CalculateMouseRay();
            SetupRayDirection();
            RotatePlayer();
            _movementFreezeTimer = ApplyStatsToAbilities.ApplyHasteSpeed(BaseMovementFreezeTime, statCollection);
            IsMovementFrozen = true;
            UpdateAnimationSpeed();
           
        }

        private void DealDamage()
        {
            var finalDamage = GetFinalDamage();
            finalDamage = ApplyStatsToAbilities.ApplyMastery(finalDamage, statCollection);
            finalDamage = ApplyStatsToAbilities.ApplyCritChance(finalDamage, statCollection);
            var colliders = Physics.OverlapSphere(slashTransform.position, damageRadius);

            foreach (var col in colliders)
            {
                if(col.TryGetComponent<IEnemyDamageable>(out var damageable))
                {
                    damageable?.TakeDamage(finalDamage);
                }
            }

            Debug.Log(finalDamage);
        }

        public void OnAttackAnimationComplete()
        {
            playerActionSate.EndAttack();

            if (_bufferedAttackInput)
            {
                _bufferedAttackInput = false;
                TriggerAutoAttack(); 
            }
            else
            {
                ResetComboIndex(); 
                playerAnimations.ResetCombo(); 
            }
        }


        private void AttackCooldownCd()
        {
            if (_bufferedAttackInput)
            {
                _bufferTimer -= Time.deltaTime;

                if (_bufferTimer <= 0f)
                {
                    _bufferedAttackInput = false;
                }
            }
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