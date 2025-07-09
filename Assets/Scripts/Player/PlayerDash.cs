using Player.Skills;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerDash : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerAnimations playerAnimations;
        [SerializeField] private Rigidbody rb;

        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashCooldown;

        public bool IsDashing { get; private set; }

        private float _dashTimer;
        private float _cooldownTimer;
        private Vector3 _dashDirection;

        private void Update()
        {
            HandleCooldowns();
        }

        private void FixedUpdate()
        {
            HandleDashMovement();
        }

        private void OnDash(InputValue value)
        {
            if (!value.isPressed) return;
            if (_cooldownTimer > 0 || IsDashing) return;

            StartDash();
        }

        private void StartDash()
        {
            _dashDirection = playerMovement.MovementDirection;
            if (_dashDirection == Vector3.zero)
            {
                _dashDirection = transform.forward; 
            }

            IsDashing = true;
            _dashTimer = dashDuration;
            _cooldownTimer = dashCooldown;

            playerAnimations.Dashing(); 
        }

        private void HandleCooldowns()
        {
            if (_cooldownTimer > 0f)
                _cooldownTimer -= Time.deltaTime;

            if (IsDashing)
            {
                _dashTimer -= Time.deltaTime;
                if (_dashTimer <= 0f)
                {
                    IsDashing = false;
                }
            }
        }

        private void HandleDashMovement()
        {
            if (IsDashing)
            {
                rb.linearVelocity = _dashDirection * dashSpeed;
            }
        }
    }
}
