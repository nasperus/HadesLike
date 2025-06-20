using System;
using System.Collections;
using Ability_System.Core_Base_Classes;
using DG.Tweening;
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


        private const float ComboMaxDelay = 2f;
        private float _lastComboTime;
        private int _comboIndex;
        
        private bool _bufferedAttackInput;
        private float _currentBufferTime;
        
        private float _attackCd;
       
        
        private float _movementFreezeTimer;
        private const float BaseMovementFreezeTime = 0.8f;

        private void Awake()
        {
            InitializeAbilities();
        }
        private void DashForward()
        {
            var dashDistance = 0.5f;
            var duration = 0.15f;
            var direction = transform.forward;
            transform.DOMove(transform.position + direction * dashDistance, duration).SetEase(Ease.OutQuad);
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
                    _currentBufferTime = 1f;
                }
                return;
            }

            TriggerAutoAttack(); 
        }
        

        private void ComboChainAttack()
        {
            if (Time.time - _lastComboTime > ComboMaxDelay)
            {
                _comboIndex = 0;
            }
            _comboIndex++;
            if (_comboIndex > 3) _comboIndex = 1;
            _lastComboTime = Time.time;

        }

        private void InitializeAbilities()
        {
            var modifier = AbilityFactory.AutoAttackDamage(damage, damageRadius);
            Init(modifier);
        }

        public void IncreaseAutoAttackDamage(float multiplier)
        {
            Stats.IncreaseStats(StatType.Damage, multiplier);
        }

      
        private void VfxPrefabSpawner()
        {
            var originalRotation = slashTransform.rotation;
            var rotated = originalRotation * Quaternion.Euler(0, -45f, 0);
            var spawnSlash = Instantiate(vfxPrefab, slashTransform.position,rotated, slashTransform);
            spawnSlash.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f, 10, 1f);
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
            VfxPrefabSpawner();
            DealDamage();
            StartCoroutine(HitStop(0.05f));
        }

        private void TriggerAutoAttack()
        {
            playerActionSate.StartAttack();
            _attackCd = ApplyStatsToAbilities.ApplyHasteSpeed(attackCooldown, statCollection);
            ComboChainAttack();
            CalculateMouseRay();
            SetupRayDirection();
            DashForward();
            RotatePlayer();
            _movementFreezeTimer = ApplyStatsToAbilities.ApplyHasteSpeed(BaseMovementFreezeTime, statCollection);
            IsMovementFrozen = true;
            UpdateAnimationSpeed();
           
        }
        private IEnumerator HitStop(float duration)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1f;
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
                    LifeSteal.GetLifeSteal(damage, playerHealth, statCollection); 
                    damageable?.TakeDamage(finalDamage);
                }
            }
        }

        public void OnAttackAnimationComplete()
        {
            playerActionSate.EndAttack();
            if (_comboIndex >= 3)
            {
                ResetComboIndex(); 
                playerAnimations.ResetCombo();
                _bufferedAttackInput = false;
                return;
            }

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
                _currentBufferTime  -= Time.deltaTime;

                if (_currentBufferTime <= 0f)
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