using UnityEngine;

namespace Player
{
    public class PlayerAnimations : MonoBehaviour
    {
       [Header("Player Movement Class")] [SerializeField] private PlayerMovement playerMovement;
       [Header("Animation Controller")] [SerializeField] private Animator animator;
       
       
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int Sprint = Animator.StringToHash("Sprint");
        private static readonly int Dash = Animator.StringToHash("Dash");
        private static readonly int Barrier = Animator.StringToHash("Barrier");
        private static readonly int Heal = Animator.StringToHash("Heal");
        private static readonly int Debuff = Animator.StringToHash("Debuff");
        private static readonly int Aoe = Animator.StringToHash("Aoe");
        private static readonly int Lightning = Animator.StringToHash("Lightning");
        private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");

        

        private void Update()
        {
            RunningAnimation();
        }

        

        private void RunningAnimation()
        {
            animator.SetBool(Running, playerMovement.IsMoving);
        }

        public void AoeDamage(float speed)
        {
            animator.SetFloat(AttackSpeed, speed);
            animator.SetTrigger(Aoe);
        }

        public void EnemyDebuff(float speed)
        {
            animator.SetFloat(AttackSpeed, speed);
            animator.SetTrigger(Debuff);
        }

        public void LightningDamage(float speed)
        {
            animator.SetFloat(AttackSpeed, speed);
            animator.SetTrigger(Lightning);
        }

        public void Healing()
        {
            animator.SetTrigger(Heal);
        }

        public void PlayerBarrier()
        {
            animator.SetTrigger(Barrier);
        }
        

        public void WeaponShoot()
        {
            animator.SetTrigger(Shoot);
        }

        public void Dashing()
        {
            animator.SetTrigger(Dash);
        }

        public void SprintIng(bool value)
        {
         animator.SetBool(Sprint, value);   
        }
    }
}
