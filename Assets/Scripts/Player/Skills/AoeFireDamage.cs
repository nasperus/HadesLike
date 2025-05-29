using System;
using System.Collections;
using Ability_System.Core_Base_Classes;
using Enemy.Archer;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Skills
{
    public class AoeFireDamage : PlayerAbilityBase
    {
        [SerializeField] private float damage;
        [SerializeField] private float tickInterval;
        [SerializeField] private float radius;
        [SerializeField] private float vfxPrefabLifetime;
        [SerializeField] private GameObject areaPrefab;
        [SerializeField] private GameObject vfxPrefabDebuff;
        [SerializeField] private float aoeCooldown;
        [SerializeField] private StatCollection statCollection;
        [SerializeField] private PlayerActionSate playerActionSate;
        [SerializeField] private PlayerMana playerMana;
        [SerializeField] private float spellCost;
        private const float BaseMovementFreezeTime = 2f;
        
        private float _movementFreezeTimer;
        private float _animationSpeed;
        
        private GameObject _aoeInstance;
        private float _aoeCooldownTimer;

        private void Awake()
        {
            InitializeAbilities();
        }

        private void InitializeAbilities()
        { var modifier = new BaseAbilityModifier(
                name: "Aoe Fire Damage",
                baseDamage: damage,
                baseRadius: 0,
                baseAoe: 0
            );
            Init(modifier);
        }

        private void OnAreaDamage(InputValue value)
        {
            if(!value.isPressed) return;
            if (playerMana.CurrentMana < spellCost) return;
            if (_aoeCooldownTimer > 0) return;
            if (playerActionSate.IsAttacking) return;
            playerActionSate.StartAttack();
            playerMana.CurrentMana -= spellCost;
            _aoeCooldownTimer = ApplyStatsToAbilities.ApplyHasteSpeed(aoeCooldown,statCollection);
            DamageEnemiesAroundClickedEnemy();
            CalculateMouseRay();
            SetupRayDirection();
            RotatePlayer();
            _movementFreezeTimer = ApplyStatsToAbilities.ApplyHasteSpeed(BaseMovementFreezeTime,statCollection);
            IsMovementFrozen = true;
            UpdateAnimationSpeed();
            
        }

        public void IncreaseAoeDamage(float multiplier)
        {
            Stats?.IncreaseStats(StatType.Damage, multiplier);;
        }

        private void UpdateAnimationSpeed()
        {
            _animationSpeed = ApplyStatsToAbilities.ApplyHasteCastAndAttackSpeed(statCollection);
            playerAnimations.AoeDamage(_animationSpeed);
        }

        private void Update()
        {
            AoeCooldownAndMovementFrozen();
        }
     
        private void AoeCooldownAndMovementFrozen()
        {
            if (_aoeCooldownTimer > 0f)
            {
                _aoeCooldownTimer -= Time.deltaTime;
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
        
        public void CastAoeDamage()
        {
            Vector3 spawnPosition = default; 
             var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
             if (Physics.Raycast(ray, out var hitInfo, 100f))
             {
                 spawnPosition = hitInfo.point;
             }
             _aoeInstance = Instantiate(areaPrefab, spawnPosition, Quaternion.identity);
             var particleSystems = _aoeInstance.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
             var adjustedLifetime = ApplyStatsToAbilities.ApplyHasteDuration(vfxPrefabLifetime, statCollection);
             foreach (var ps in particleSystems)
             {
                 var goName = ps.gameObject.name;
                 if (goName is "Circle")
                 {
                     var main  = ps.main;
                     main.startLifetime  = adjustedLifetime;
                 }
             }
           
             StartCoroutine(ApplyAoeDamage(spawnPosition, _aoeInstance));
        }

        private IEnumerator ApplyAoeDamage(Vector3 position, GameObject aoeVfx)
        {
            var elapsed = 0f;
            var finalDamage = GetFinalDamage();
            finalDamage = ApplyStatsToAbilities.ApplyMastery(finalDamage,statCollection);
            finalDamage = ApplyStatsToAbilities.ApplyCritChance(finalDamage,statCollection);
            var adjustedTickInterval = ApplyStatsToAbilities.ApplyHasteSpeed(tickInterval,statCollection);
            var adjustedLifetime = ApplyStatsToAbilities.ApplyHasteDuration(vfxPrefabLifetime, statCollection);
            
            while (elapsed < adjustedLifetime)
            {
                var colliders = Physics.OverlapSphere(position, radius);
                foreach (var coll in colliders)
                {
                    if (coll.TryGetComponent<IEnemyDamageable>(out var damageable))
                    { 
                       var debuff = Instantiate(vfxPrefabDebuff, coll.transform.position, Quaternion.identity, coll.transform);
                        Destroy(debuff, 1f);
                        damageable?.TakDamage(finalDamage);
                    }
                }
                yield return new WaitForSeconds(adjustedTickInterval);
                elapsed += adjustedTickInterval;
            }
            Destroy(aoeVfx);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

    }
    
   
}
