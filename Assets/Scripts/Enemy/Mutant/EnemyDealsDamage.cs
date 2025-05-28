using UnityEngine;

namespace Enemy.Mutant
{
    public class EnemyDealsDamage : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float capsuleRadius;

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
