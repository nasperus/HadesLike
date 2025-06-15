using Enemy.Mutant;
using Prayers;
using UnityEngine;

namespace Enemy.Archer
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private int arrowDamage;
        private const int Miss = 4;


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IPlayerDamageable>(out var damageable))
            {
                var missAttack = CriticalMiss.ShouldMiss(Miss);
                if (missAttack == true)
                {
                    Destroy(gameObject);
                    return;
                }
                damageable?.TakeDamage(arrowDamage);
                Destroy(gameObject);
            }
        }
    }
}
