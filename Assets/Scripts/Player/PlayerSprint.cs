using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerSprint : MonoBehaviour
    {
       [Header("Player Movement Class")] [SerializeField] private PlayerMovement playerMovement;
       [Header("Player Animation Class")] [SerializeField] private PlayerAnimations playerAnimations;
       [Header("Player Input Controller")] [SerializeField] private PlayerInput playerInput;
       [Header("RigidBody")] [SerializeField] private Rigidbody rb;
       [Header("Sprint Speed")] [SerializeField] private float sprintSpeed;
        
        private InputAction _sprintAction;

        public float SprintSpeed => sprintSpeed;
        public  bool IsSprinting { get; private set; }
        
        
        private void Awake()
        {
            _sprintAction = playerInput.actions["Sprint"];
        }

        public void UpdateSprintSpeed(float newSprintSpeed)
        {
            sprintSpeed = newSprintSpeed;
        }

        private void OnEnable()
        {
            _sprintAction.started += PlayerPressSprint;
            _sprintAction.canceled += PlayerReleaseSprint;
        }
        
        private void OnDisable()
        {
            _sprintAction.started -= PlayerPressSprint;
            _sprintAction.canceled -= PlayerReleaseSprint;
        }
        
        private void PlayerPressSprint(InputAction.CallbackContext context)
        {
            IsSprinting = true;
            playerAnimations.SprintIng(true);
        }
        private void PlayerReleaseSprint(InputAction.CallbackContext context)
        {
            IsSprinting = false;
            playerAnimations.SprintIng(false);
        }
    }
}
