using System.Collections.Generic;
using Enemy.Mutant;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
   public class EnemyMovement : MonoBehaviour
   {
      [SerializeField] private Rigidbody rb;
      [SerializeField] private float movementSpeed;
      [SerializeField] private Transform player;
      [SerializeField] private float rotationSpeed;
      [SerializeField] private EnemyAnimations enemyAnimations;
      [SerializeField]private float attackCooldown;
      
      private float _attackTimer;
      private Vector3 _movement;
      private NavMeshAgent _agent;
      
      private List<(float level, int value)> _distanceLevels = new()
      {
         (2f, 0),
         (4f, 1),
         (5f, 2)
      };

      private enum EnemyState
      {
         Idle,
         Chasing,
         Attacking
      }
      private EnemyState _currentState = EnemyState.Idle;
      
      private void Awake()
      {
         _agent = GetComponent<NavMeshAgent>();
         _agent.updatePosition = false;
         _agent.updateRotation = false;

      }
      private void Start()
      {
         rb.freezeRotation = true;
         
         
      }

      private void FixedUpdate()
      {
         if (player == null) return;
         _agent.SetDestination(player.position);
         
         DistanceToPlayer();
         
         if (_currentState == EnemyState.Chasing)
         {
            EnemyHasPath();
         }
         _agent.nextPosition = transform.position;
         
         if (_currentState == EnemyState.Attacking)
         {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    
            _attackTimer -= Time.fixedDeltaTime;
            
            if (_attackTimer <= 0f)
            {
               //enemyAnimations.MeleeAttack();
               _attackTimer = attackCooldown;
            }
         }
      }
      
      private void DistanceToPlayer()
      {
         var distanceToPlayer = Vector3.Distance(transform.position, player.position);
         foreach (var (level, value) in _distanceLevels)
         {
            if (distanceToPlayer <= level)
            {
               HandleDistanceLevel(value);
               return;
            }
         }
         if (_currentState != EnemyState.Chasing)
         {
            _currentState = EnemyState.Chasing;
            enemyAnimations.EnemyRunning(true);
         }
         
      }

      private void HandleDistanceLevel(int level)
      {
         switch (level)
         {
            case 0:
               if (_currentState != EnemyState.Attacking)
               {
                  //Debug.Log("Melee Attack");
                   _currentState = EnemyState.Attacking;
                   enemyAnimations.EnemyRunning(false);
                   _attackTimer = attackCooldown;
                   enemyAnimations.MeleeAttack();
                   rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
               }
               break;
            case 1:
               Debug.Log("Case1");
               break;
            case 2:
               Debug.Log("Case2");
               break;
         }
      }

      private void EnemyHasPath()
      {
         if (_agent.hasPath)
         {
            var desiredVelocity = _agent.desiredVelocity;
            desiredVelocity.y = 0;
            _movement = desiredVelocity.normalized * movementSpeed;
            rb.linearVelocity  = new Vector3(_movement.x, rb.linearVelocity.y, _movement.z);
            
            if (_movement != Vector3.zero)
            {
               enemyAnimations.EnemyRunning(true);
               var targetRotation = Quaternion.LookRotation(_movement);
               transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
           
         }
      }
   }
}