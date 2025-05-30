using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ability_System.Enum;
using Enemy.Archer;
using Enemy.Mutant;
using Prayers;

namespace Portals
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject meleeEnemyPrefabs;
        [SerializeField] private GameObject rangedEnemyPrefabs;
        [SerializeField] private float delayBetweenSpawns = 2f;
        
        private Transform _playerRangeHitPoint;
        private Transform _playerTransform;
   

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        public void SetPlayerTransform(Transform player, Transform playerHitPoint)
        {
            _playerTransform = player;
            _playerRangeHitPoint = playerHitPoint;
        }
        
        private IEnumerator Spawn()
        {
            var allEnemies = new List<GameObject>();
            allEnemies.Add(meleeEnemyPrefabs);
            allEnemies.Add(rangedEnemyPrefabs);
            
            if(allEnemies.Count == 0)
            {
                yield break;
            }
            var randomIndex = Random.Range(0, allEnemies.Count);
            var prefab = allEnemies[randomIndex];
                
            var enemy = Instantiate(prefab, transform.position, Quaternion.identity);
            
            if (enemy.TryGetComponent<EnemyStateMachine>(out var stateMachine))
            {
                stateMachine.GetPlayerTransform(_playerTransform);
                if (stateMachine is ArcherStateMachine archer)
                {
                    archer.SetPlayerRangeHitPoint(_playerRangeHitPoint);
                }
            }
           
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }
}
