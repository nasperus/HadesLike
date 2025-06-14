using System.Collections;
using UnityEngine;

namespace Room_Generation
{
    public class RoomExit : MonoBehaviour
    {
        private RoomManager _roomManager;
        private bool _isActive = false;

        private void Start()
        {
            _roomManager = FindFirstObjectByType<RoomManager>();
        }

        public void ActivateExit()
        {
            _isActive = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive) return;

            if (other.CompareTag("Player"))
            {
                _roomManager.PlayerEnteredPortal();
                Destroy(gameObject); 
            }
        }
        
        private IEnumerator DelayedRoomLoad()
        {
            yield return new WaitForSeconds(2f);
            _roomManager.PlayerEnteredPortal();
            Destroy(gameObject); 
        }
    }
}