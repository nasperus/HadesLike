using System.Collections;
using System.Collections.Generic;
using Enemy.Archer;
using Enemy.Mutant;
using Room_Generation;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject meleeEnemyPrefabs;
    [SerializeField] private GameObject rangedEnemyPrefabs;

    private Transform _playerRangeHitPoint;
    private Transform _playerTransform;

    private bool _hasSpawned = false;
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
        if (_hasSpawned) yield break;
        _hasSpawned = true;

        yield return new WaitForSeconds(0.5f); 

        var allEnemies = new[] { meleeEnemyPrefabs, rangedEnemyPrefabs };
        var randomIndex = Random.Range(0, allEnemies.Length);
        var prefab = allEnemies[randomIndex];

        var enemy = Instantiate(prefab, transform.position, Quaternion.identity);
        EnemyTracker.Instance?.AddEnemiesToTrack(1); 

        if (enemy.TryGetComponent<EnemyStateMachine>(out var stateMachine))
        {
            stateMachine.GetPlayerTransform(_playerTransform);
            if (stateMachine is ArcherStateMachine archer)
            {
                archer.SetPlayerRangeHitPoint(_playerRangeHitPoint);
            }
        }

        Destroy(gameObject); 
    }
}

