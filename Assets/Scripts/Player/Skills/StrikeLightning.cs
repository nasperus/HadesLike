using Enemy.Archer;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

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
        
        private float _skillCooldownTimer;
        private const float BaseMovementFreezeTime = 2f;
        public bool IsMovementFrozen { get; private set; }
        private float _movementFreezeTimer;
       
        
        private void Update()
        {
            AoeCooldownAndMovementFrozen();
         
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
           var finalDamage = ApplyStatsToAbilities.ApplyMastery(skillDamageAmount, statCollection);
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
           
           var colliders = Physics.OverlapSphere(spawnPosition, radius); 

            foreach (var coll in colliders)
            {
                if (coll.TryGetComponent<IEnemyDamageable>(out var damageable))
                {
                    damageable?.TakDamage(finalDamage);
                }
            }
            Destroy(vfxInstance, vfxPrefabLifetime);
       }

    }
}
