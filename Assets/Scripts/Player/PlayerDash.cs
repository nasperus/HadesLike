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
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashCooldown = 1f;
        [SerializeField] private float dashDistance;

        private Vector3 _dashStartPosition;
        private Vector3 _dashDirection;
        private float _cooldownTimer;

        public bool IsDashing { get; private set; }

        private void Update()
        {
            HandleCooldown();
        }

        private void FixedUpdate()
        {
            HandleDashMovement();
        }

        private void OnDash(InputValue value)
        {
            if (!value.isPressed) return;
            if (_cooldownTimer > 0f || IsDashing) return;

            StartDashWithoutPressingDirection();
        }

        private void StartDashWithoutPressingDirection()
        {
            _dashDirection = playerMovement.MovementDirection;
            if (_dashDirection == Vector3.zero)
                _dashDirection = transform.forward;
            
            IsDashing = true;
            _dashStartPosition = transform.position;
            _cooldownTimer = dashCooldown;
            
            playerAnimations.Dashing();
        }

        private void HandleCooldown()
        {
            if (_cooldownTimer > 0f)
                _cooldownTimer -= Time.deltaTime;
        }

        private void HandleDashMovement()
        {
            if (!IsDashing) return;

            var distanceDashed = Vector3.Distance(_dashStartPosition, transform.position);
            if (distanceDashed >= dashDistance)
            {
                var endPoint = _dashStartPosition + _dashDirection.normalized * dashDistance;
                transform.position = endPoint;
                
                IsDashing = false;
                rb.linearVelocity = Vector3.zero;
            }
            rb.linearVelocity = _dashDirection * dashSpeed;
        }
    }
}
