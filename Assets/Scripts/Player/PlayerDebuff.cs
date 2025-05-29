using System.Collections.Generic;
using System.Linq;
using Ability_System.Core_Base_Classes;
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


        private void Awake()
        {
            InitializeAbilities();
        }

        private void InitializeAbilities()
        {
            var modifier = new BaseAbilityModifier(
                name: "Debuff",
                baseDamage: tickDamage,
                baseRadius: 0,
                baseAoe: 0
                );
            
            Init(modifier);
        }

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


        public void IncreaseDotDamage(float multiplier)
        {
            Stats?.IncreaseStats(StatType.Damage, multiplier);
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
            var newTickDamage = GetFinalDamage();
            if (ClosestEnemy.TryGetComponent<IEnemyDamageable>(out _))
            {
                StatCalculator.CalculateStats(statCollection,newTickDamage,tickInterval, 
                    out _mastery, out _haste, out _critical);
                
                var dot = ClosestEnemy.GetComponent<DebuffDamage>();
                if (dot == null)
                {
                    dot = ClosestEnemy.gameObject.AddComponent<DebuffDamage>();
                }
                dot.Init(newTickDamage, _haste, dotDuration, statCollection);
                
                var alreadyHitEnemies = new List<Transform> { ClosestEnemy.transform };
                ChainDot(ClosestEnemy.transform, alreadyHitEnemies);
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

        private void ChainDot(Transform originEnemy, List<Transform> alreadyHitEnemies)
        {
            const int chainRadius = 10;
            const int maxChain = 5;

            var nearbyEnemies = Physics.OverlapSphere(originEnemy.position, chainRadius)
                .Where(c => c.TryGetComponent<IEnemyDamageable>(out _) 
                            && !alreadyHitEnemies.Contains(c.transform))
                .OrderBy(c => Vector3.Distance(originEnemy.position, c.transform.position))
                .Take(maxChain).ToList();

            foreach (var nearby in nearbyEnemies)
            {
                if (nearby.TryGetComponent<IEnemyDamageable>(out _))
                {
                    var dot = nearby.GetComponent<DebuffDamage>();
                    if (dot == null)
                    {
                        dot = nearby.gameObject.AddComponent<DebuffDamage>();
                    }
                    dot.Init(GetFinalDamage(), _haste, dotDuration, statCollection);
                    alreadyHitEnemies.Add(nearby.transform);
                    
                    if (!nearby.TryGetComponent<DebuffVFXHandler>(out var handler))
                    {
                        var vfxInstance = Instantiate(vfxPrefab, nearby.transform.position, Quaternion.identity, nearby.transform);
                        vfxInstance.transform.localPosition = Vector3.zero;

                        handler = nearby.gameObject.AddComponent<DebuffVFXHandler>();
                        handler.Init(vfxInstance.GetComponent<ParticleSystem>(), dotDuration);
                    }
                    else
                    {
                        handler.ResetTimer(dotDuration);
                    }
                }
            }
        }

        
    }
    
}