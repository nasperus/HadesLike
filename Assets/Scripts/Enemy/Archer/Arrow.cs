using Prayers;
using UnityEngine;

namespace Enemy.Archer
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private int arrowDamage;
        private int miss = 4;
       

         
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IPlayerDamageable>(out var damageable))
            {
                if (CriticalMiss.ShouldMiss(miss))
                {
                    Debug.Log($"{gameObject.name} MISSED their attack due to miss chance ({CriticalMiss.GetMissChance(miss)}%)");
                    
                }
                damageable?.TakeDamage(arrowDamage);
                Destroy(gameObject);
            }
        }
    }
}
