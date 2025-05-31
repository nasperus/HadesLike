using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace Room_Generation
{
    public class GenerateLevel : MonoBehaviour
    {
        [SerializeField] private RoomData[] roomPrefabs; 
        [SerializeField] private int numberOfRooms;
        [SerializeField] private NavMeshSurface surface;

        
        private List<Vector2Int> placedRooms = new List<Vector2Int>();
        private Dictionary<Vector2Int, GameObject> spawnedRooms = new Dictionary<Vector2Int, GameObject>();

        private Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
        };

        private void Awake()
        {
            LevelGenerator();
            surface.BuildNavMesh();
        }

      private void LevelGenerator()
      {
          ClearLevel();
            var currentPosition = Vector2Int.zero;
            placedRooms.Add(currentPosition);
            SpawnRoom(currentPosition);

            for (int i = 1; i < numberOfRooms; i++)
            {
                Vector2Int newPos = Vector2Int.zero;
                bool found = false;

                for (int attempt = 0; attempt < 100; attempt++)
                {
                    Vector2Int baseRoom = placedRooms[Random.Range(0, placedRooms.Count)];
                    Vector2Int dir = directions[Random.Range(0, directions.Length)];
                    newPos = baseRoom + dir;

                    if (!placedRooms.Contains(newPos))
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    placedRooms.Add(newPos);
                    SpawnRoom(newPos);
                }
            }
        }
        private void ClearLevel()
        {
            foreach (var room in spawnedRooms.Values)
            {
                Destroy(room);
            }

            spawnedRooms.Clear();
            placedRooms.Clear();
        }



        private void SpawnRoom(Vector2Int gridPos)
        {
            var roomData = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            var prefabSize = roomData.size;
            var worldPos = new Vector3(gridPos.x * prefabSize.x, 0, gridPos.y * prefabSize.z);

            var room = Instantiate(roomData.prefab, worldPos, Quaternion.identity, transform);
            spawnedRooms.Add(gridPos, room);
        }


       private Vector3 GetPrefabSize(GameObject prefab)
        {
            var renderer = prefab.GetComponentInChildren<Renderer>();
            return renderer ? renderer.bounds.size : new Vector3(15f, 0f, 15f); 
        }
    }
}
