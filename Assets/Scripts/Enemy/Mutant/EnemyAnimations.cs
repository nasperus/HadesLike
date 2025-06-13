using UnityEngine;

namespace Enemy.Mutant
{
   public class EnemyAnimations : MonoBehaviour
   {
      [SerializeField] private Animator animator;
   
   
      private static readonly int Hit = Animator.StringToHash("Hit");
      private static readonly int Run = Animator.StringToHash("Run");
      private static readonly int Shoot = Animator.StringToHash("Shoot");
      private static readonly int Melee = Animator.StringToHash("Melee");
      private static readonly int LegKick = Animator.StringToHash("LegKick");
      private static readonly int Death = Animator.StringToHash("Death");
      private static readonly int IceMeteor = Animator.StringToHash("IceMeteor");



      public void KickLeg()
      {
         animator.SetTrigger(LegKick);
      }

      public void MeteorIce()
      {
         animator.SetTrigger(IceMeteor);
      }

      public void EnemyHit()
      {
         animator.SetTrigger(Hit);
      }

      public void EnemyDeath()
      {
         animator.SetTrigger(Death);
      }

      public void EnemyShoot()
      {
         animator.SetTrigger(Shoot);
      }

      public void EnemyRunning(bool value)
      {
         animator.SetBool(Run,value);
      }

      public void MeleeAttack()
      {
         animator.SetTrigger(Melee);
      }


   }
}
