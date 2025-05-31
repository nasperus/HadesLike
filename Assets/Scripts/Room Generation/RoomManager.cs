using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UI;
using Unity.Cinemachine;

namespace Room_Generation
{
    public class RoomManager : MonoBehaviour
    {
        [Header("Room Setup")]
        [SerializeField] private RoomData[] roomPrefabs;
        [SerializeField] private NavMeshSurface navMeshSurface;

        [Header("Player Setup")]
        [SerializeField] private GameObject playerPrefab;
        
        

        private GameObject _currentRoom;
        private GameObject _playerInstance;

        private void Start()
        {
            SpawnNewRoom(); 
        }
        

        public void SpawnNewRoom()
        {
            if (_currentRoom != null)
            {
                Destroy(_currentRoom);
            }

            var roomData = GetRandomRoom();
            _currentRoom = Instantiate(roomData.prefab, Vector3.zero, Quaternion.identity);
            var spawnPoint = _currentRoom.transform.Find("PlayerSpawnPoint");

            if (spawnPoint == null)
            {
                Debug.LogError("No PlayerSpawnPoint found in room prefab!");
                return;
            }

            if (_playerInstance == null)
            {
                _playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                _playerInstance.transform.position = spawnPoint.position;
            }

            var virtualCamera = FindFirstObjectByType<CinemachineVirtualCameraBase>();
            if (virtualCamera != null)
            {
                virtualCamera.Follow = _playerInstance.transform;
                virtualCamera.LookAt = _playerInstance.transform;
            }

            GameManager.Instance.PlayerSpawned(_playerInstance);

            var powerUpButtons = FindFirstObjectByType<PowerUpButtons>();
            if (powerUpButtons != null)
            {
                powerUpButtons.SetPlayerReferences(_playerInstance);
            }

            if (navMeshSurface != null)
            {
                navMeshSurface.BuildNavMesh();
            }
            EnemyTracker.Instance?.ResetTracker();

            StartCoroutine(SetupPortalSpawnersAfterDelay()); 
        }

        private IEnumerator SetupPortalSpawnersAfterDelay()
        {
            yield return new WaitForSeconds(2f); 

            var playerRange = _playerInstance.transform.Find("Range");
            if (playerRange == null)
            {
                Debug.LogError("Could not find 'Range' child under player!");
                yield break;
            }

            var portalSpawners = FindObjectsOfType<PortalSpawner>();
            foreach (var portalSpawner in portalSpawners)
            {
                portalSpawner.SetPlayerTransform(_playerInstance.transform, playerRange);
                portalSpawner.ResetSpawner();
            }
        }

        private RoomData GetRandomRoom()
        {
            return roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        }
    }
}