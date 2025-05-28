using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerRotation : MonoBehaviour
    {
       [FormerlySerializedAs("playerMouseDirection")] [Header("Player MouseDirection Class")] [SerializeField] private PlayerLeftClickAttack playerLeftClickAttack;
       [Header("Player Mouse Shoot Class")] [SerializeField] private PlayerDebuff playerShoot;
       [Header("Player Movement Class")] [SerializeField] private PlayerMovement playerMovement;
       [Header("Rotation Speed")] [SerializeField] private float rotationSpeed;
    
        
        private void FixedUpdate()
        {
            PlayerRotationLogic();
        }

        private void PlayerRotationLogic()
        {
                if (playerLeftClickAttack.MouseClickLookTimer > 0 && 
                    playerLeftClickAttack.MouseClickTargetRotationQuaternion.HasValue)
                {
                    playerLeftClickAttack.MouseClickLookTimer -= Time.fixedDeltaTime;
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation, playerLeftClickAttack.MouseClickTargetRotationQuaternion.Value, 
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
