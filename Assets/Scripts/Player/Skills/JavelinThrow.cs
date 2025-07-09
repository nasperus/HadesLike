using System;
using Enemy.Archer;
using Stats;
using UnityEngine;

namespace Player.Skills
{
    public class JavelinThrow : MonoBehaviour
    {
         private float _damage;
        [SerializeField] private StatCollection statCollection;
        [SerializeField] private PlayerThrowSkill skill;

        public void SetBaseDamage(float damage)
        {
            _damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IEnemyDamageable>(out var damageable))
            {
                damageable.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }

      
    }
}
