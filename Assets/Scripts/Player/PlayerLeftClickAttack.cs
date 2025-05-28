using Player.Skills;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerLeftClickAttack : PlayerAbilityBase
    {
        [SerializeField] private PlayerActionSate playerActionSate;
        
        private void OnMouseClickLeft(InputValue value)
        {
            if (!value.isPressed) return;
            IsLeftClicking = true;
            if (playerActionSate.IsAttacking) return;
            playerActionSate.StartAttack();
            DamageEnemiesAroundClickedEnemy();
            CalculateMouseRay();
            SetupRayDirection();
            RotatePlayer();
            playerAnimations.PlayerAutoAttack();
            
        }

        public void TriggerAutoAttack()
        {
            Debug.Log("triggered");
        }
        
        
    }
}