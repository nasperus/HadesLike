using TMPro;
using Player.Skills;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
       [Header("Player Sprint Class")] [SerializeField] private PlayerSprint playerSprint;
       [Header ("Player MouseDirection Class")] [SerializeField] private PlayerLeftClickAttack playerLeftClickAttack;
       [Header("Player Dash Class")] [SerializeField] private PlayerDash playerDash;
       [Header("Player Shoot Class")] [SerializeField] private PlayerDebuff playerShoot;
       [Header("RigidBody")] [SerializeField] private Rigidbody rb;
       [Header("Movement Speed")] [SerializeField] private float movementSpeed;
       [Header("Acceleration Speed")] [SerializeField] private float acceleration;
       [Header("Shooting Movement Speed")] [SerializeField] private float shootingMovementSpeed;
       [SerializeField] private StatCollection statCollection;
        
        private Camera _mainCamera;
        private Vector2 _playerInput;
        private Vector2 _currentInput;
        private AoeFireDamage _aoeFireDamage;
        private StrikeLightning _strikeLightning;
        
        public float MovementSpeed => movementSpeed;
        public Vector3 Movement { get; private set; }
        
        public bool IsMoving {get; private set;}
        private bool _wasIdle;

        private void Awake()
        {
            _aoeFireDamage = GetComponent<AoeFireDamage>();
            _strikeLightning = GetComponent<StrikeLightning>();
        }

        public void UpdateMovementSpeed()
        {
            movementSpeed = statCollection.GetStatValue(StatTypeEnum.MovementSpeed);
        }

        private void Start()
        {
            rb.freezeRotation = true;
            _mainCamera = Camera.main;
        }

        private void FixedUpdate()
        {
            PlayerMouseClickAndDashMethods();
            
        }

        private float GetCalculatedMovementSpeed(float baseSpeed)
        {
            return  ApplyStatsToAbilities.ApplyMovementSpeedBonus(baseSpeed, statCollection);
        }

        private void SetVelocity(Vector3 velocity)
        {
            rb.linearVelocity = velocity;
            IsMoving = velocity.sqrMagnitude > 0.001f;
        }
        
        private void OnMove(InputValue value) {_currentInput = value.Get<Vector2>();}
      
        
        private void CameraRelativeToPlayerDirection()
        {
            var forward = _mainCamera.transform.forward;
            var right =_mainCamera.transform.right;
        
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
        
            Movement = (right * _playerInput.x + forward * _playerInput.y).normalized;
        }
        
        private void PlayerAccelerationAndSprintMovementSpeedLogic()
        {
            if (playerDash.IsDashing) return;
            var baseWalkSpeed = movementSpeed;
            var walkSpeedWithBonus = GetCalculatedMovementSpeed(baseWalkSpeed);
            var sprintSpeedWithBonus = walkSpeedWithBonus  + playerSprint.SprintSpeed;
            var currentSpeed = playerSprint.IsSprinting ? sprintSpeedWithBonus : walkSpeedWithBonus;
            
                
            var targetVelocity = Movement * currentSpeed;

            if (!IsMoving)
            {
                rb.linearVelocity = Vector3.zero;
                _wasIdle = true;
                return;
            }

            if (playerShoot.IsRightClicking)
            {
                rb.linearVelocity = Movement * currentSpeed;
                return;
            }
            if (_wasIdle)
            { 
                rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
                   
                if ((rb.linearVelocity - targetVelocity).sqrMagnitude < 0.01f)
                { 
                    _wasIdle = false;
                }
            }
            else
            { 
                rb.linearVelocity = targetVelocity;
            }
        }
        
        private void PlayerMouseClickAndDashMethods()
        {
            if (playerDash.IsDashing)
            {
                IsMoving = false;
                return; 
            }
            _playerInput = _currentInput;
            CameraRelativeToPlayerDirection();
            IsMoving = Movement.sqrMagnitude > 0.001f;
            
            if (playerLeftClickAttack.PausePlayerMovementDuringClick > 0f  
                && playerLeftClickAttack.IsLeftClicking)
            {
                playerLeftClickAttack.PausePlayerMovementDuringClick -= Time.fixedDeltaTime;
                playerLeftClickAttack.ResetMouseLeftClickFlag();
                SetVelocity(Vector3.zero);
                IsMoving = false;
                return; 
            }

            if (_aoeFireDamage.IsMovementFrozen ||_strikeLightning.IsMovementFrozen)
            {
                SetVelocity(Vector3.zero);
                IsMoving = false;
                
            }
            
            PlayerAccelerationAndSprintMovementSpeedLogic();
        }
    }
}
