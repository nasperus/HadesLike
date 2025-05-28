using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMouseDirection : MonoBehaviour
    {
        [SerializeField] private PlayerAnimations playerAnimations;
        [SerializeField] StatCollection statCollection;
        
        private Camera _mainCamera;
        public Quaternion? MouseClickTargetRotationQuaternion { get; set; } = null;

        //stops movement when the mouse clicked
        public float PausePlayerMovementDuringClick { get; set; }
        private  float PauseMovementDurationDuringClick { get; set; } = 0.2f;

        //stop WASD rotation during mouse click
        public float MouseClickLookTimer { get; set; }
        private  float MouseClickLookDuration { get; set; } =  0.1f;
        public bool IsLeftClicking { get; private set; }
       
        
        private void Start()
        {
            _mainCamera = Camera.main;
        }

       
        private void OnMouseClickLeft(InputValue value)
        {
            if (!value.isPressed) return;
          
            
            IsLeftClicking = true;
            PausePlayerMovementDuringClick = PauseMovementDurationDuringClick;
            
            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hitInfo, 100f))
            {
                var lookPoint = hitInfo.point;
                lookPoint.y = transform.position.y;
                var direction = (lookPoint - transform.position).normalized;

                if (direction.sqrMagnitude > 0.001f)
                {
                    MouseClickTargetRotationQuaternion = Quaternion.LookRotation(direction);
                    MouseClickLookTimer = MouseClickLookDuration;
                }
            }
        }
        
        public void ResetMouseLeftClickFlag()
        {
            if (PausePlayerMovementDuringClick <= 0f)
            {
                IsLeftClicking = false;
                
            }
        }
    }
}

