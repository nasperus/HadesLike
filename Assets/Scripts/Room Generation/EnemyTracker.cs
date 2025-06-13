using UnityEngine;
using System;

namespace Room_Generation
{
    public class EnemyTracker : MonoBehaviour
    {
        public static EnemyTracker Instance;

        private int _totalEnemiesToSpawn;
        private int _enemiesKilled;
        private bool _isSpawningComplete = false;
        public static event Action OnRoomCleared;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            ResetTracker();
        }

        public void AddEnemiesToTrack(int count)
        {
            _totalEnemiesToSpawn += count;
        }

        public void SetSpawningComplete()
        {
            _isSpawningComplete = true;
            CheckRoomCleared();
        }

        public void RegisterEnemyDeath()
        {
            _enemiesKilled++;
            CheckRoomCleared();
        }

        private void CheckRoomCleared()
        {
            if (_isSpawningComplete && _enemiesKilled >= _totalEnemiesToSpawn && _totalEnemiesToSpawn > 0)
            {
                OnRoomCleared?.Invoke();
            }
        }
        
        public void ResetTracker()
        {
            _totalEnemiesToSpawn = 0;
            _enemiesKilled = 0;
            _isSpawningComplete = false;
        }
    }
}