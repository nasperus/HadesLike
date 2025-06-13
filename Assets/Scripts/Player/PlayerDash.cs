using Player.Skills;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerDash : MonoBehaviour
    {
       [Header("Player Movement Class")] [SerializeField] private PlayerMovement playerMovement;
       [Header("Player Animation Class")] [SerializeField] private PlayerAnimations playerAnimations;
       [Header("Dash Speed")] [SerializeField] private float dashSpeed;
       [Header("Dash Duration")] [SerializeField] private float dashDuration;
       [Header("Dash Cooldown")] [SerializeField] private float dashCooldown;
       [Header("RigidBody")] [SerializeField] private Rigidbody rb;
       [SerializeField] private PlayerActionSate playerActionSate;
        
        public bool IsDashing { get; private set; }
        
        private float _dashTimer = 0; 
        private float _cooldownTimer = 0;
        
        private Vector3 _dashDirection; 
        private void FixedUpdate()
        {
            DashTimersLogic();
           
        }
        private void OnDash(InputValue value)
        {
            if (!value.isPressed) return;
            if (_cooldownTimer > 0) return;
            //if (playerActionSate.IsAttacking) return;
            //playerActionSate.StartAttack();
            IsDashing = true;
            var dashDirection = playerMovement.Movement;
                
            if (dashDirection == Vector3.zero)
            {
                dashDirection = transform.forward;
            }
            playerAnimations.Dashing();
            rb.linearVelocity = dashDirection * dashSpeed;
            _dashTimer = dashDuration;
            _cooldownTimer = dashCooldown;
            
        }
        
        private void DashTimersLogic()
        {
            if (_cooldownTimer > 0)
                _cooldownTimer -= Time.deltaTime;
            
            if (IsDashing)
            {
                _dashTimer -= Time.deltaTime;
                if (_dashTimer <= 0)
                {
                    IsDashing = false;
                    
                }
            }
        }
    }
}
