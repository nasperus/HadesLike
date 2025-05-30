using System;
using System.Net;
using Ability_System.Enum;
using Enemy.Mutant;
using Prayers;
using UnityEngine;

namespace Enemy.Archer
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private int arrowDamage;
        private BoonRarity boonRarity = BoonRarity.Legendary;

         
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IPlayerDamageable>(out var damageable))
            {
                if (boonRarity.ShouldMiss())
                {
                    Debug.Log($"{gameObject.name} MISSED their attack due to miss chance ({CriticalMiss.GetMissChance(boonRarity)}%)");
                    
                }
                damageable?.TakeDamage(arrowDamage);
                Destroy(gameObject);
            }
        }
    }
}
