using UnityEngine;

namespace Room_Generation
{
    public class EnemyTracker : MonoBehaviour
    {
        public static EnemyTracker Instance;

        private int totalEnemiesToSpawn;
        private int enemiesKilled;

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

        public void RegisterEnemyDeath()
        {
            enemiesKilled++;
            if (enemiesKilled >= totalEnemiesToSpawn)
            {
                Debug.Log("Room Cleared");
            }
        }

        public void ResetTracker()
        {
            totalEnemiesToSpawn = 0;
            enemiesKilled = 0;
        }
    }
}