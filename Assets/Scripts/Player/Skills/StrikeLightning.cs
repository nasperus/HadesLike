using System.Collections;
using Ability_System.Core_Base_Classes;
using Enemy.Archer;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

namespace Player.Skills
{
    public class StrikeLightning : PlayerAbilityBase
    {
        [SerializeField] private int skillDamageAmount;
        [SerializeField] private GameObject vfxPrefab;
        [SerializeField] private float vfxPrefabLifetime;
        [SerializeField] private StatCollection statCollection;
        [SerializeField] private float skillCooldown;
        [SerializeField] private PlayerActionSate playerActionSate;
        [SerializeField] private PlayerMana playerMana;
        [SerializeField] private float spellCost;
        [SerializeField]  private float radius;
        [SerializeField] private float chainDelay;  
        [SerializeField] private int maxChains;
        [SerializeField] private int chainRadius;
        
        private float _skillCooldownTimer;
        private const float BaseMovementFreezeTime = 2f;
        private float _movementFreezeTimer;
     
        
        private void Update()
        {
            AoeCooldownAndMovementFrozen();
         
        }

         private  void Awake()
        {
            InitializeAbilities();
        }

        private void InitializeAbilities()
        {
            var modifier = new BaseAbilityModifier(
                name: "Lightning Strike",
                baseDamage: skillDamageAmount,
                baseRadius: radius,
                baseAoe: 0
            );
            
            Init(modifier);
        }
        
        private void OnLightningStrike(InputValue value)
       {
           if (!value.isPressed) return;
           if (playerMana.CurrentMana < spellCost) return;
           if (_skillCooldownTimer > 0) return;
           if (playerActionSate.IsAttacking) return;
           playerActionSate.StartAttack();
           playerMana.CurrentMana -= spellCost;
           _skillCooldownTimer =  ApplyStatsToAbilities.ApplyHasteSpeed(skillCooldown,statCollection);
           DamageEnemiesAroundClickedEnemy();
           CalculateMouseRay();
           SetupRayDirection();
           RotatePlayer();
           _movementFreezeTimer = ApplyStatsToAbilities.ApplyHasteSpeed(BaseMovementFreezeTime,statCollection);
           IsMovementFrozen = true;
           UpdateAnimationSpeed();
           
       }

        public void IncreaseLightningStrikeDamage(float multiplier)
        {
            Stats?.IncreaseStats(StatType.Damage, multiplier);
        }

        public void IncreaseLightningStrikeRadius(float multiplier)
        {
            Stats?.IncreaseStats(StatType.Radius, multiplier);
        }
       
        private void UpdateAnimationSpeed()
        {
            var animationSpeed = ApplyStatsToAbilities.ApplyHasteCastAndAttackSpeed(statCollection);
            playerAnimations.LightningDamage(animationSpeed);
           
        }
        
        public void TriggerLightningAttack()
        {
            CastLightning();
        }
        
       private void AoeCooldownAndMovementFrozen()
       {
           if (_skillCooldownTimer > 0f)
           {
               _skillCooldownTimer -= Time.deltaTime;
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
       
       private void CastLightning()
       {
           Vector3 spawnPosition = default;
           var finalDamage = GetFinalDamage();

           finalDamage = ApplyStatsToAbilities.ApplyMastery(finalDamage, statCollection);
           finalDamage = ApplyStatsToAbilities.ApplyCritChance(finalDamage, statCollection);

           if (ClosestEnemy != null)
           {
               spawnPosition = ClosestEnemy.ClosestPoint(ShootOrigin);
           }
           else
           {
               var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
               if (Physics.Raycast(ray, out var hitInfo, 100f))
               {
                   spawnPosition = hitInfo.point;
               }
           }

           var vfxInstance = Instantiate(vfxPrefab, spawnPosition, Quaternion.identity);
           var finalRadius = GetFinalRadius();
           var colliders = Physics.OverlapSphere(spawnPosition, finalRadius); 

           List<Transform> alreadyHitEnemies = new List<Transform>();

           foreach (var coll in colliders)
           {
               if (coll.TryGetComponent<IEnemyDamageable>(out var damageable))
               {
                   damageable?.TakDamage(finalDamage);
                   alreadyHitEnemies.Add(coll.transform);
                   //boon implement here
                   StartCoroutine(ChainLightningCoroutine(coll.transform, finalDamage * 0.3f,
                       alreadyHitEnemies, maxChains));
               }
           }

           Destroy(vfxInstance, vfxPrefabLifetime);
       }
    

       private IEnumerator ChainLightningCoroutine(Transform origin, float baseDamage, List<Transform> alreadyHitEnemies,
           int remainingChains)
       {
           if (remainingChains <= 0) yield break;
           
           yield return new WaitForSeconds(chainDelay);

           var nearbyTargets = Physics.OverlapSphere(origin.position, chainRadius)
               .Where(c => c.TryGetComponent<IEnemyDamageable>(out _) && !alreadyHitEnemies.Contains(c.transform))
               .OrderBy(c => Vector3.Distance(origin.position, c.transform.position))
               .ToList();

           foreach (var target in nearbyTargets)
           {
               if (target.TryGetComponent<IEnemyDamageable>(out var damageable))
               {
                   damageable?.TakDamage(baseDamage);
                   alreadyHitEnemies.Add(target.transform);
                   var chainVfx = Instantiate(vfxPrefab, target.transform.position, Quaternion.identity);
                   Destroy(chainVfx, vfxPrefabLifetime);
                   StartCoroutine(ChainLightningCoroutine(target.transform, baseDamage, alreadyHitEnemies, remainingChains - 1));
                   break; 
               }
           }
       }


    }
}

