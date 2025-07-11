using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerRotation : MonoBehaviour
    {
        [FormerlySerializedAs("playerMouseDirection")]
        [Header("Player MouseDirection Class")] 
        [SerializeField] private PlayerLeftClickAttack playerLeftClickAttack;

        [Header("Player Mouse Shoot Class")] 
        [SerializeField] private PlayerDebuff playerShoot;

        [Header("Player Movement Class")] 
        [SerializeField] private PlayerMovement playerMovement;

        [Header("Rotation Speed")] 
        [SerializeField] private float rotationSpeed;
        [SerializeField] private PlayerDash playerDash;

        private void Update()
        {
            PlayerRotationLogic();
        }

        private void PlayerRotationLogic()
        {
            if (playerDash.IsDashing) return; 

            if (playerLeftClickAttack.MouseClickLookTimer > 0 &&
                playerLeftClickAttack.MouseClickTargetRotationQuaternion.HasValue)
            {
                playerLeftClickAttack.MouseClickLookTimer -= Time.deltaTime;
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    playerLeftClickAttack.MouseClickTargetRotationQuaternion.Value,
                    rotationSpeed * Time.deltaTime);
                return;
            }

            if (playerShoot.IsRightClicking) return;

            if (!playerMovement.IsMoving) return;

            var direction = playerMovement.MovementDirection;
            if (direction.sqrMagnitude < 0.01f) return;

            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }
}