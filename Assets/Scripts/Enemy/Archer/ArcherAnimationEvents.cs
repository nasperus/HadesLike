using UnityEngine;

namespace Enemy.Archer
{
    public class ArcherAnimationEvents : MonoBehaviour
    {
        private ArcherAttackState _archerAttackState;

        public void Init(ArcherAttackState attackState)
        {
            _archerAttackState = attackState;
        }

        public void ShootArrowEvent()
        {
            _archerAttackState?.ShootArrow();
        }

        public void LegKick()
        {
            _archerAttackState?.LegKickDamage();
        }

        public void OnShootEnd()
        {
            _archerAttackState.OnShootAnimationEnd();
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_archerAttackState == null) return;
            Gizmos.color = Color.red;
            var center = transform.position;
            var radius = _archerAttackState.GetKickRadius();
            Gizmos.DrawWireSphere(center, radius);
        }
    }
}
