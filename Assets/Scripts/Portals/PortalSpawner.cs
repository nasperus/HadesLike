using System.Collections;
using Room_Generation;
using UnityEngine;
using UnityEngine.AI;

public class PortalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject portalSpawnerPrefab;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float minDistanceFromPlayer = 3f;
    [SerializeField] private Transform playerTransform;
    private Transform _playerRangeHitPoint;


      
    public void SetPlayerTransform(Transform player, Transform hitPoint)
    {
        playerTransform = player;
        _playerRangeHitPoint = hitPoint;
        StartCoroutine(SpawnPortals()); 
    }
    
    public void ResetSpawner()
    {
        StopAllCoroutines(); 
        StartCoroutine(SpawnPortals()); 
    }
        
    private IEnumerator SpawnPortals()
    {
        for (var i = 0; i < 3; i++)
        { 
            var spawnPoint = FindValidSpawnPoint();

            if (spawnPoint != Vector3.zero)
            {
                var portal = Instantiate(portalSpawnerPrefab, spawnPoint, Quaternion.identity);
                var spawner = portal.GetComponent<EnemySpawner>();
                    
                if (spawner != null)
                {
                    spawner.SetPlayerTransform(playerTransform,_playerRangeHitPoint);
                }
            }
            yield return new WaitForSeconds(2);
        }
        yield return new WaitForSeconds(1f);
        EnemyTracker.Instance?.SetSpawningComplete();
    }

    private Vector3 FindValidSpawnPoint()
    {
        for (var i = 0; i < 10; i++)
        {
            var randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection.y = 0;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out var hit, spawnRadius, NavMesh.AllAreas))
            {
                if (Vector3.Distance(hit.position, playerTransform.position) >= minDistanceFromPlayer)
                {
                    return hit.position;
                }
            }
        }
        return Vector3.zero;
    }
}