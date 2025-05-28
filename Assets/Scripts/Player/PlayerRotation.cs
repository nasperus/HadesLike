using UnityEngine;

namespace Player
{
    public class PlayerRotation : MonoBehaviour
    {
       [Header("Player MouseDirection Class")] [SerializeField] private PlayerMouseDirection playerMouseDirection;
       [Header("Player Mouse Shoot Class")] [SerializeField] private PlayerDebuff playerShoot;
       [Header("Player Movement Class")] [SerializeField] private PlayerMovement playerMovement;
       [Header("Rotation Speed")] [SerializeField] private float rotationSpeed;
    
        
        private void FixedUpdate()
        {
            PlayerRotationLogic();
        }

        private void PlayerRotationLogic()
        {
                if (playerMouseDirection.MouseClickLookTimer > 0 && 
                    playerMouseDirection.MouseClickTargetRotationQuaternion.HasValue)
                {
                    playerMouseDirection.MouseClickLookTimer -= Time.fixedDeltaTime;
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation, playerMouseDirection.MouseClickTargetRotationQuaternion.Value, 
                        rotationSpeed * Time.fixedDeltaTime);
                    return;
                }
                
                if (playerShoot.IsRightClicking) return;
                
                if (!playerMovement.IsMoving) return;
                    var targetRotation = Quaternion.LookRotation(playerMovement.Movement);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                        rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
