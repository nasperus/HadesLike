using Enemy.Mutant;
using UnityEngine;

namespace Enemy.Warlock_Boss.Skills
{
    public class SkillDamage : MonoBehaviour
    {
       
            [SerializeField] private int damage = 10;
            [SerializeField] private float radius = 3f;
            [SerializeField] private float delay = 0.5f;
            [SerializeField] private LayerMask playerLayer;
        
            private readonly Collider[] _results = new Collider[5];
        
            private void Start()
            {
                Invoke(nameof(DealDamage), delay);
            }
        
            private void DealDamage()
            {
                var hitCount = Physics.OverlapSphereNonAlloc(transform.position, radius, _results, playerLayer);
        
                for (var i = 0; i < hitCount; i++)
                {
                    var collider = _results[i];
                    if (collider.TryGetComponent<IPlayerDamageable>(out var player))
                    {
                        player.TakeDamage(damage);
                        Debug.Log($"<color=red>{gameObject.name}</color> hit <color=yellow>{player}</color> for <b>{damage}</b> damage.");
                    }
                }
            }
    }
}
