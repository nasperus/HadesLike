using UnityEngine;

namespace Enemy.Archer
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private int arrowDamage;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IPlayerDamageable>(out var damageable))
            {
                damageable?.TakeDamage(arrowDamage);
                Destroy(gameObject);
            }
        }
    }
}
