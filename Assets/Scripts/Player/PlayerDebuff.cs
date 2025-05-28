using Enemy.Archer;
using Player.Skills;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;
using VFX;

namespace Player
{
    public class PlayerDebuff : PlayerAbilityBase
    {
        [SerializeField] private GameObject vfxPrefab;
        [SerializeField] private int tickDamage;
        [SerializeField] private float tickInterval;
        [SerializeField] private float dotDuration;
        [SerializeField] private StatCollection statCollection;
        [SerializeField] private PlayerStatsManager playerStatsManager;
        [SerializeField] private PlayerActionSate playerActionSate;
        [SerializeField] private PlayerMana playerMana;
        [SerializeField] private float spellCost;
        private float _mastery;
        private float _haste;
        private float _critical;
        public bool IsRightClicking { get; private set; }
        
        private void OnMouseClickRight(InputValue value)
        {
           
            if (!value.isPressed) return;
            if (playerMana.CurrentMana < spellCost) return;
            if (playerActionSate.IsAttacking) return;
            DamageEnemiesAroundClickedEnemy();
            CalculateMouseRay();
            SetupRayDirection();
            if (ClosestEnemy == null) return;
            if (!ClosestEnemy.TryGetComponent<IEnemyDamageable>(out _)) return;
            playerActionSate.StartAttack();
            playerMana.CurrentMana -= spellCost;
            RotatePlayer();
            UpdateAnimationSpeed(); 
        }
        

        private void UpdateAnimationSpeed()
        {
            var animationSpeed = ApplyStatsToAbilities.ApplyHasteCastAndAttackSpeed(statCollection);
            playerAnimations.EnemyDebuff(animationSpeed);
        }

        public void TriggerDebuffAnimation()
        {
            DebuffEnabled();
        }
        
        private void DebuffEnabled()
        {
            if (ClosestEnemy == null) return;
            if (ClosestEnemy.TryGetComponent<IEnemyDamageable>(out _))
            {
                StatCalculator.CalculateStats(statCollection,tickDamage,tickInterval, 
                    out _mastery, out _haste, out _critical);
                
                var dot = ClosestEnemy.GetComponent<DebuffDamage>();
                if (dot == null)
                {
                    dot = ClosestEnemy.gameObject.AddComponent<DebuffDamage>();
                }
                dot.Init(tickDamage, _haste, dotDuration, statCollection);
            }
            
            if (!ClosestEnemy.TryGetComponent<DebuffVFXHandler>(out var handler))
            {
                var hitPosition = ClosestEnemy.ClosestPoint(ShootOrigin);
                var vfxInstance = Instantiate(vfxPrefab, hitPosition, Quaternion.identity, ClosestEnemy.transform);
                vfxInstance.transform.localPosition = Vector3.zero;

                handler = ClosestEnemy.gameObject.AddComponent<DebuffVFXHandler>();
                handler.Init(vfxInstance.GetComponent<ParticleSystem>(), dotDuration);
            }
            else
            {
                handler.ResetTimer(dotDuration);
            }
        }
        
    }
    
    }