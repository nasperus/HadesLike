using TMPro;
using Player.Skills;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Player Abilities")]
        [SerializeField] private PlayerSprint playerSprint;
        [SerializeField] private PlayerLeftClickAttack playerLeftClickAttack;
        [SerializeField] private PlayerDash playerDash;
        [SerializeField] private PlayerDebuff playerShoot;

        [Header("Physics")]
        [SerializeField] private Rigidbody rb;

        [Header("Movement Settings")]
        [SerializeField] private float baseMovementSpeed;
        

        [Header("Stat System")]
        [SerializeField] private StatCollection statCollection;
        private float _statMovementSpeed;
        private Vector2 _input;
        private Vector3 _movementDirection;
        private Camera _mainCamera;
        private Vector3 _velocityRef;

        private PlayerAbilityBase[] _playerAbilities;

        private float _calculatedMovementSpeed;

        public bool IsMoving { get; private set; }

        public float MovementSpeed => _calculatedMovementSpeed;
        public Vector3 MovementDirection => _movementDirection;

        private void Awake()
        {
            rb.freezeRotation = true;
            _playerAbilities = GetComponents<PlayerAbilityBase>();
            
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            UpdateMovementSpeed(); 
        }

        private void Update()
        {
            
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                
            }
        }

        private void FixedUpdate()
        {
            CalculateCameraRelativeDirection();
            HandleMovement();
        }

        
        public void UpdateMovementSpeed()
        {
            _statMovementSpeed = statCollection.GetStatValue(StatTypeEnum.MovementSpeed);
            
            if (_statMovementSpeed <= 0f)
            {
                _statMovementSpeed = baseMovementSpeed;
            }
            _calculatedMovementSpeed = ApplyStatsToAbilities.ApplyMovementSpeedBonus(_statMovementSpeed, statCollection);
        }

        
        private void OnMove(InputValue value)
        {
            _input = value.Get<Vector2>();
        }
        

        private void CalculateCameraRelativeDirection()
        {
            if (playerDash.IsDashing) return;
            var forward = _mainCamera.transform.forward;
            var right = _mainCamera.transform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            _movementDirection = (right * _input.x + forward * _input.y).normalized;
        }

        private void HandleMovement()
        {
            if (IsMovementFrozen() || playerDash.IsDashing)
            {
                StopMovement();
                return;
            }

            if (playerLeftClickAttack.IsLeftClicking && playerLeftClickAttack.PausePlayerMovementDuringClick > 0f)
            {
                playerLeftClickAttack.PausePlayerMovementDuringClick -= Time.fixedDeltaTime;
                playerLeftClickAttack.ResetMouseLeftClickFlag();
                StopMovement();
                return;
            }

            var isTryingToMove = _movementDirection.sqrMagnitude > 0.001f;
            IsMoving = isTryingToMove;

            if (!isTryingToMove)
            {
                StopMovement();
                return;
            }
            
            var sprintBonus = playerSprint.IsSprinting ? playerSprint.SprintSpeed : 0f;
            var finalSpeed = _calculatedMovementSpeed + sprintBonus;

            var targetVelocity = _movementDirection * finalSpeed;

            if (playerShoot.IsRightClicking || playerSprint.IsSprinting)
            {
                rb.linearVelocity = targetVelocity;
            }
            else
            {
                 rb.linearVelocity = Vector3.SmoothDamp(
                     rb.linearVelocity,
                     targetVelocity,
                     ref _velocityRef,
                     0.01f,
                     float.MaxValue,
                     Time.fixedDeltaTime
                );
            }
        }
        
        private void StopMovement()
        {
            rb.linearVelocity = Vector3.zero;
            IsMoving = false;
        }
        
        private bool IsMovementFrozen()
        {
            foreach (var ability in _playerAbilities)
            {
                if (ability.IsMovementFrozen)
                    return true;
            }
            return false;
        }
        
        
    }
}
