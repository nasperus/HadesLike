using UnityEngine;
using UnityEngine.InputSystem;


namespace Player
{
    public class PlayerSprint : MonoBehaviour
    {
        [Header("Player Movement Class")]
        [SerializeField] private PlayerMovement playerMovement;
        [Header("Player Animation Class")]
        [SerializeField] private PlayerAnimations playerAnimations;
        [Header("Player Input Controller")]
        [SerializeField] private PlayerInput playerInput;
        [Header("Sprint Speed")]
        [SerializeField] private float sprintSpeed;

        private InputAction _sprintAction;
        private bool _isSprintKeyPressed;

        public float SprintSpeed => sprintSpeed;
        public bool IsSprinting { get; private set; }

        
        private bool ShouldSprint => _isSprintKeyPressed && playerMovement.IsMoving;

        private void Awake()
        {
            _sprintAction = playerInput.actions["Sprint"];
        }
        
        private void Update()
        {
            UpdateSprintState();
        }

        private void OnEnable()
        {
            _sprintAction.started += OnSprintPressed;
            _sprintAction.canceled += OnSprintReleased;
        }

        private void OnDisable()
        {
            _sprintAction.started -= OnSprintPressed;
            _sprintAction.canceled -= OnSprintReleased;
        }

        private void OnSprintPressed(InputAction.CallbackContext context)
        {
            _isSprintKeyPressed = true;
            UpdateSprintState();
        }

        private void OnSprintReleased(InputAction.CallbackContext context)
        {
            _isSprintKeyPressed = false;
            UpdateSprintState();
        }
        
        private void UpdateSprintState()
        {
            var newSprintState = ShouldSprint;

            if (newSprintState != IsSprinting)
            {
                IsSprinting = newSprintState;
                playerAnimations.SprintIng(IsSprinting);
            }
        }
        
        public void UpdateSprintSpeed(float newSprintSpeed)
        {
            sprintSpeed = newSprintSpeed;
        }
    }
}
