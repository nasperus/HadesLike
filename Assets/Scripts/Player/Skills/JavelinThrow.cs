using System;
using Enemy.Archer;
using UnityEngine;

namespace Player.Skills
{
    public class JavelinThrow : MonoBehaviour
    {
        [SerializeField] private float damage;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IEnemyDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage);
                Debug.Log("TakeDamage");
            }
        }
    }
}
