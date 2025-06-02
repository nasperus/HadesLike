using UnityEngine;

namespace Room_Generation
{
    public class RoomExit : MonoBehaviour
    {
        private RoomManager _roomManager;

        private void Start()
        {
            _roomManager = FindFirstObjectByType<RoomManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _roomManager.SpawnNewRoom();
                Destroy(gameObject);
            }
        }
    }
}