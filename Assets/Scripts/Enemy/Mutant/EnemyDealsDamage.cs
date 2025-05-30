using System;
using Ability_System.Enum;
using Prayers;
using UnityEngine;

namespace Enemy.Mutant
{
    public class EnemyDealsDamage : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float capsuleRadius;
        private BoonRarity boonRarity = BoonRarity.Legendary;
       
        

        private Collider[] _results = new Collider[5];
        
        public void DealDamage()
        { 
            var center = transform.position;
            var hitCount = Physics.OverlapSphereNonAlloc(center, capsuleRadius, _results, playerLayer);
           
           for (var i = 0; i < hitCount; i++)
           {
               var hitCollider = _results[i];

               if (hitCollider.TryGetComponent<IPlayerDamageable>(out var damageable))
               {
                   if (boonRarity.ShouldMiss())
                   {
                       Debug.Log($"{gameObject.name} MISSED their attack due to miss chance ({CriticalMiss.GetMissChance(boonRarity)}%)");
                       continue;
                   }
                   damageable?.TakeDamage(damage);
               }
               
           }
        }

     
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, capsuleRadius);
        }

    }
}
