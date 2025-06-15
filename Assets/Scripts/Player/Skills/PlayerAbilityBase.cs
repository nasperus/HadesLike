using Ability_System.Core_Base_Classes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Skills
{
    public abstract class PlayerAbilityBase: Abilities
    {
        [Header("References")]
        [SerializeField] protected PlayerLeftClickAttack playerLeftClickAttack;
        [SerializeField] protected PlayerAnimations playerAnimations;
        [SerializeField] protected Transform weaponTransformOrigin; 

        [Header("Shooting Settings")]
        [SerializeField] protected float weaponRange;
        [SerializeField] protected float rayWidth; 
        [SerializeField] protected LayerMask enemyLayerMask;

        // [Header("Ray Colors")]
        // [SerializeField] protected bool drawDebugLines = true;
        // [SerializeField] protected Color rayColor = Color.red;
        // [SerializeField] protected Color hitColor = Color.green;
        
        [Header("Rotation Durations")]
        [SerializeField] private float enemyClickLookDuration;
        [SerializeField] private float floorClickLookDuration;
        
        public Quaternion? MouseClickTargetRotationQuaternion { get; set; } = null;

        //stops movement when the mouse clicked
        public float PausePlayerMovementDuringClick { get; set; }

        //stop WASD rotation during mouse click
        public float MouseClickLookTimer { get; set; }
        public bool IsLeftClicking { get; protected set; }
        public bool IsMovementFrozen { get; protected set; }

        private bool _clickedEnemy;

        private Camera _mainCamera;
        private Ray _mouseRay;
        private Plane _groundPlane;
        private Vector3 _targetPosition;
        private Vector3 _shootDirection;
        private Vector3 _lookDirection;
        private Vector3 _endPoint;
        private Collider[] _potentialTargets;
        protected Collider ClosestEnemy;
        protected Vector3 ShootOrigin;
        private const int MaxTargets = 10;

        protected virtual void Start()
        {
            _mainCamera = Camera.main;
        }
      
        
        
        public void ResetMouseLeftClickFlag()
        {
            if (PausePlayerMovementDuringClick <= 0f)
            {
                IsLeftClicking = false;
            }
            
        }

        protected void CalculateMouseRay()
        {
            _mouseRay = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            _groundPlane = new Plane(Vector3.up, Vector3.zero);
            _targetPosition = _mouseRay.GetPoint(100f);
            
            if (_groundPlane.Raycast(_mouseRay, out var distance))
            {
                _targetPosition = _mouseRay.GetPoint(distance);
            }
        }

        protected void SetupRayDirection()
        {
            ShootOrigin = weaponTransformOrigin.position;
            _shootDirection = (_targetPosition - ShootOrigin).normalized;
            _endPoint = ShootOrigin + (_shootDirection * weaponRange);

            // if (drawDebugLines)
            //     Debug.DrawLine(ShootOrigin, _endPoint, rayColor, 2f);
        }

        protected void RotatePlayer()
        {
            _lookDirection = _shootDirection.normalized;

            if (_lookDirection.sqrMagnitude > 0.001f)
            {
                var targetRotation = Quaternion.LookRotation(_lookDirection);
                 transform.DORotateQuaternion(targetRotation, 0.15f).SetEase(Ease.OutQuad);
                 playerLeftClickAttack.MouseClickTargetRotationQuaternion = targetRotation;
                
                if (_clickedEnemy)
                {
                    playerLeftClickAttack.MouseClickLookTimer = enemyClickLookDuration;
                    playerLeftClickAttack.PausePlayerMovementDuringClick = enemyClickLookDuration;
                }
                else
                {
                    playerLeftClickAttack.MouseClickLookTimer = floorClickLookDuration;
                    playerLeftClickAttack.PausePlayerMovementDuringClick = floorClickLookDuration;
                }
            }
        }


        protected void DamageEnemiesAroundClickedEnemy()
        {
            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out var hit, weaponRange, enemyLayerMask))
            {
                var clickedObject = hit.collider;

                if (clickedObject.CompareTag("Enemy"))
                {
                    ClosestEnemy = clickedObject;
                    _clickedEnemy = true;
                    return;
                }
            }

            ClosestEnemy = null;
            _clickedEnemy = false;
        }

        
        // protected void FindClosestEnemy()
        // {
        //     _potentialTargets = new Collider[MaxTargets];
        //     var hitCount = Physics.OverlapCapsuleNonAlloc(ShootOrigin, _endPoint, rayWidth, _potentialTargets, enemyLayerMask);
        //
        //     var minDistance = float.MaxValue;
        //     ClosestEnemy = null;
        //
        //     for (var i = 0; i < hitCount; i++)
        //     {
        //         var col = _potentialTargets[i];
        //         if (!col.CompareTag("Enemy")) continue;
        //
        //         var dist = Vector3.Distance(ShootOrigin, col.transform.position);
        //         if (dist < minDistance)
        //         {
        //             minDistance = dist;
        //             ClosestEnemy = col;
        //         }
        //     }
        // }
    }
} 