using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Skills
{
    public class HealingPowerUpActivator : MonoBehaviour
    {
        [SerializeField] private PlayerAnimations playerAnimations;
        [SerializeField] private PlayerDash playerDash;
        [SerializeField] private float healAmount = 25f;
        [SerializeField] private GameObject healVFXPrefab;
        [SerializeField] private float healingTimer;
        private float _healingCoolDown;

        private PlayerHealth _playerHealth;

        private void Awake()
        {
            _playerHealth = GetComponent<PlayerHealth>();
        }

        private void Update()
        {
            HealingTimer();
        }

        private void OnHeal(InputValue value)
        {
            if (!value.isPressed) return;
            if (playerDash.IsDashing) return;
            if (_healingCoolDown > 0) return;

            if (_playerHealth != null)
            {
                _playerHealth.Heal(healAmount);
                Debug.Log("Healing PowerUp");
                if (healVFXPrefab != null)
                {
                    playerAnimations.Healing();
                    var obj = Instantiate(healVFXPrefab, transform.position, Quaternion.identity, transform);
                    Destroy(obj, 1.5f);
                }
                _healingCoolDown = healingTimer;
            }
        }
        
        private void HealingTimer()
        {
            if (_healingCoolDown > 0)
            {
                _healingCoolDown -= Time.deltaTime;
            }
        }
    }
}