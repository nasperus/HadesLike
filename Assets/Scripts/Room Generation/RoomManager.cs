using System;
using System.Collections;
using Enemy.Warlock_Boss;
using Stats;
using Unity.AI.Navigation;
using UnityEngine;
using UI;
using Unity.Cinemachine;
using Random = UnityEngine.Random;

namespace Room_Generation
{
  
    public class RoomManager : MonoBehaviour
    {
        [Header("Room Setup")]
        [SerializeField] private RoomData[] roomPrefabs;
        [SerializeField] private GameObject bossRoomPrefab;
        [SerializeField] private int totalNormalRooms;
        [SerializeField] private NavMeshSurface navMeshSurface;

        [Header("Player Setup")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject exitVfxPrefab;
        [SerializeField] private PowerUpChoicePanel powerUpPanel;
        
        private GameObject _currentRoom;
        private GameObject _playerInstance;
        private int _roomsCleared = 0;
        public static float IncreaseEnemyHealth { get; private set; } = 0;
        public static float IncreaseAttackSpeed { get; private set; } = 0;


        private void Awake()
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 240;
        }

        private void Start()
        {
            SpawnNewRoom();
            //StartCoroutine(RunAgain());

        }

        private IEnumerator RunAgain()
        {
            while (true)
            {
                powerUpPanel.ShowRandomPowerUps(5);
                yield return new WaitForSeconds(2);
            }

        }

        private void OnEnable()
        {
            EnemyTracker.OnRoomCleared += RoomCleared;
        }

        private void OnDisable()
        {
            EnemyTracker.OnRoomCleared -= RoomCleared;
        }

        private void RoomCleared()
        {
            var exitSpawn = _currentRoom.transform.Find("Exit");
            if (exitSpawn == null)
            {
                Debug.LogError("Exit spawn point not found in room!");
                return;
            }

            var spawnPosition = exitSpawn.position + Vector3.up * 1f;
            var rotation = Quaternion.Euler(0, 90, 0);
            var exitObject = Instantiate(exitVfxPrefab, spawnPosition, rotation);

            var roomExit = exitObject.GetComponent<RoomExit>();
            if (roomExit != null)
            {
                roomExit.ActivateExit();
                powerUpPanel.ShowRandomPowerUps(5);
            }
        
        }
        
        public void PlayerEnteredPortal()
        {
            _roomsCleared++;
            IncreaseEnemyHealth += 3;
            IncreaseAttackSpeed += 0.1f;
            IncreaseAttackSpeed = Mathf.Max(IncreaseAttackSpeed, 0.5f); 
            IncreaseEnemyHealth = Mathf.Min(IncreaseEnemyHealth, 30f);
            
            if (_roomsCleared < totalNormalRooms)
                SpawnNewRoom();
            else
                SpawnBossRoom();
        }
        

        private void SpawnNewRoom()
        {
            if (_currentRoom != null)
            {
                Destroy(_currentRoom);
            }

            var roomData = GetRandomRoom();
            _currentRoom = Instantiate(roomData.prefab, Vector3.zero, Quaternion.identity);
            SetupRoom();
        }

        private void SpawnBossRoom()
        {
            if (_currentRoom != null)
            {
                Destroy(_currentRoom);
            }

            _currentRoom = Instantiate(bossRoomPrefab, Vector3.zero, Quaternion.identity);
            SetupRoom();
        }

        private void SetupRoom()
        {
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
            var statDisplayPanel = FindFirstObjectByType<StatDisplayPanel>();
            if (statDisplayPanel != null)
            {
                statDisplayPanel.SetPlayerReferences(_playerInstance);
            }

            var warlocks = FindFirstObjectByType<WarlockStateMachine>();
            if (warlocks != null)
            {
                warlocks.GetPlayerTransform(_playerInstance.transform);
            }

            var virtualCamera = FindFirstObjectByType<CinemachineVirtualCameraBase>();
            if (virtualCamera != null)
            {
                virtualCamera.Follow = _playerInstance.transform;
                virtualCamera.LookAt = _playerInstance.transform;
            }

            var powerUpButtons = FindFirstObjectByType<PowerUpButtons>();
            if (powerUpButtons != null)
            {
                powerUpButtons.SetPlayerReferences(_playerInstance);
            }

            // var surface = _currentRoom.GetComponent<NavMeshSurface>();
            // if (surface != null)
            // {
            //     surface.BuildNavMesh();
            // }

            EnemyTracker.Instance?.ResetTracker();
            StartCoroutine(SetupPortalSpawnersAfterDelay());
        }

        private IEnumerator SetupPortalSpawnersAfterDelay()
        {
            yield return new WaitForSeconds(2f);

            var playerRange = _playerInstance.transform.Find("Range");
            if (playerRange == null)
            {
                Debug.LogError("Could not find Range child");
                yield break;
            }

            var portalSpawners = FindFirstObjectByType<PortalSpawner>();
            if (portalSpawners != null)
            {
                portalSpawners.SetPlayerTransform(_playerInstance.transform, playerRange);
                portalSpawners.ResetSpawner();
            }
            
        }

        private RoomData GetRandomRoom()
        {
            return roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        }
    }
}
