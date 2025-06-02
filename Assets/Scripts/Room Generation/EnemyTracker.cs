using UnityEngine;
using System;

namespace Room_Generation
{
    public class EnemyTracker : MonoBehaviour
    {
        public static EnemyTracker Instance;

        private int totalEnemiesToSpawn;
        private int enemiesKilled;
        private bool isSpawningComplete = false;
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
            totalEnemiesToSpawn += count;
        }

        public void SetSpawningComplete()
        {
            isSpawningComplete = true;
            CheckRoomCleared();
        }

        public void RegisterEnemyDeath()
        {
            enemiesKilled++;
            CheckRoomCleared();
        }

        private void CheckRoomCleared()
        {
            if (isSpawningComplete && enemiesKilled >= totalEnemiesToSpawn && totalEnemiesToSpawn > 0)
            {
                OnRoomCleared?.Invoke();
            }
        }
            

        public void ResetTracker()
        {
            totalEnemiesToSpawn = 0;
            enemiesKilled = 0;
            isSpawningComplete = false;
        }
    }
}