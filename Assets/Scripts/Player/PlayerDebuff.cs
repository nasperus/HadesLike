using System.Collections;
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
        [SerializeField] private float chainDelay; 
        [SerializeField] private int maxChains;
        [SerializeField] private int chainRadius;
       
        public bool IsRightClicking { get; private set; }

        private bool _activateChainDot = false;
        
        
        private void Awake()
        {
            InitializeAbilities();
        }

        private void InitializeAbilities()
        {
            var modifier = AbilityFactory.DotDamage(tickDamage);
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

        public void ActivateChainDot()
        {
            _activateChainDot = true;
        }

        private void UpdateAnimationSpeed()
        {
            var animationSpeed = ApplyStatsToAbilities.ApplyHasteCastAndAttackSpeed(statCollection);
            playerAnimations.EnemyDebuff(animationSpeed);
        }

        public void TriggerDebuffAnimation()
        {
            TriggerDebuffChain();
        }
        
        private void TriggerDebuffChain()
        {
            if (ClosestEnemy == null) return;
            
            var alreadyHitEnemies = new List<Transform> { ClosestEnemy.transform };
            StartCoroutine(ChainDebuffCoroutine(ClosestEnemy.transform, alreadyHitEnemies, maxChains));
        }

        private IEnumerator ChainDebuffCoroutine(Transform originEnemy, List<Transform> alreadyHitEnemies,
            int remainingChains)
        {
            if (remainingChains <= 0) yield break;
        
            var baseDotDamage = GetFinalDamage();

            var finalDamage = ApplyStatsToAbilities.ApplyMastery(baseDotDamage, statCollection);
            finalDamage = ApplyStatsToAbilities.ApplyCritChance(finalDamage, statCollection);
            var adjustedTickInterval = ApplyStatsToAbilities.ApplyHasteSpeed(tickInterval, statCollection);

            if (originEnemy.TryGetComponent<IEnemyDamageable>(out _))
            {
                var dot = originEnemy.GetComponent<DebuffDamage>();
                if (dot == null)
                {
                    dot = originEnemy.gameObject.AddComponent<DebuffDamage>();
                }
                dot.Init(finalDamage, adjustedTickInterval, dotDuration, statCollection);
            }
        
            if (!originEnemy.TryGetComponent<DebuffVFXHandler>(out var handler))
            {
                var vfxInstance = Instantiate(vfxPrefab, originEnemy.position, Quaternion.identity, originEnemy);
                vfxInstance.transform.localPosition = Vector3.zero;

                handler = originEnemy.gameObject.AddComponent<DebuffVFXHandler>();
                handler.Init(vfxInstance.GetComponent<ParticleSystem>(), dotDuration);
            }
            else
            {
                handler.ResetTimer(dotDuration);
            }

            yield return new WaitForSeconds(chainDelay);
            if (_activateChainDot)
            {
                var nearbyEnemies = Physics.OverlapSphere(originEnemy.position, chainRadius)
                    .Where(c => c.TryGetComponent<IEnemyDamageable>(out _)
                                && !alreadyHitEnemies.Contains(c.transform))
                    .OrderBy(c => Vector3.Distance(originEnemy.position, c.transform.position))
                    .ToList();

                if (nearbyEnemies.Count > 0)
                {
                    var nextEnemy = nearbyEnemies[0].transform;
                    alreadyHitEnemies.Add(nextEnemy);
            
                    StartCoroutine(ChainDebuffCoroutine(nextEnemy, alreadyHitEnemies, 
                        remainingChains - 1));
                }
            }
           
        }
        
    }
       
}
        
    
    
