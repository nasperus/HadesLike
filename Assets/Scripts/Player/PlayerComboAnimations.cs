using UnityEngine;

namespace Player
{
    public class PlayerComboAnimations : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string idleStateName = "Idle";
        [SerializeField] private string[] comboStates = { "Barrier", "Buff", "SlashAttack" };
        [SerializeField] private float comboResetTime = 1.5f;
        
        private int _comboIndex = 0;
        private float _comboTimer = 0;
        private CombatState _currentState = CombatState.Idle;
        
        private bool _inputReceived = false;
        
        private enum CombatState
        {
            Idle,
            Attacking,
            ComboWindow
        }
        
        private void Update()
        {
            HandleInput();
            HandleComboReset();
        }
        
        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_currentState == CombatState.Idle)
                {
                    _comboIndex = 0;
                    PlayAnimation(comboStates[_comboIndex]);
                    _currentState = CombatState.Attacking;
                }
                else if (_currentState == CombatState.Attacking || _currentState == CombatState.ComboWindow)
                {
                    _inputReceived = true;
                    //Debug.Log("Input received for next combo");
                }
            }
        }
      
        private void PerformNextComboAttack()
        {
            _inputReceived = false;
            _comboIndex++;
            
            if (_comboIndex < comboStates.Length)
            {
                PlayAnimation(comboStates[_comboIndex]);
                _currentState = CombatState.Attacking;
                //Debug.Log("Performing next combo attack: " + comboStates[_comboIndex]);
            }
            else
            {
                // We've reached the end of the combo sequence
                ResetCombo();
            }
        }
        
          
        // Called by animation event when combo window starts
        public void EnterComboWindow()
        {
            _currentState = CombatState.ComboWindow;
            _comboTimer = 0f;
            
            // If player has already input the next attack, process it immediately
            if (_inputReceived)
            {
                PerformNextComboAttack();
            }
            
            //Debug.Log("Entered combo window");
        }

        
        // Called by animation event when animation ends
        public void EndAttack()
        {
            //Debug.Log("EndAttack called, current state: " + _currentState);
            
            if (_currentState == CombatState.ComboWindow)
            {
                // If we're in the combo window and player has input the next attack
                if (_inputReceived)
                {
                    PerformNextComboAttack();
                }
                else
                {
                    // No input received during combo window, return to idle
                    ResetCombo();
                }
            }
            else if (_currentState == CombatState.Attacking)
            {
                ResetCombo();
            }
        }
        
        private void HandleComboReset()
        {
            if (_currentState == CombatState.ComboWindow)
            {
                _comboTimer += Time.deltaTime;
                if (_comboTimer >= comboResetTime)
                {
                    ResetCombo();
                }
            }
        }
        
        private void ResetCombo()
        {
            _currentState = CombatState.Idle;
            _comboIndex = 0;
            _comboTimer = 0f;
            _inputReceived = false;
            
            
            animator.CrossFade(idleStateName, 0.1f);
            
            //Debug.Log("Combo reset, returning to " + idleStateName);
        }
        
        private void PlayAnimation(string stateName)
        {
            //Debug.Log("Playing animation: " + stateName);
            animator.CrossFade(stateName, 0.1f);
            _comboTimer = 0f;
        }
    }
}