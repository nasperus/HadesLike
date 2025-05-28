using UnityEngine;

namespace Player.Skills
{
    public class PlayerActionSate : MonoBehaviour
    {
      public bool IsAttacking {get; private set;}

      public void StartAttack()
      {
          IsAttacking = true;
      }

      public void EndAttack()
      {
          IsAttacking = false;
      }
    }
}
