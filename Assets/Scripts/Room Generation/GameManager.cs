using System;
using Stats;
using UI;
using UnityEngine;

namespace Room_Generation
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public StatDisplayPanel statDisplayPanel;  

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void PlayerSpawned(GameObject player)
        {
            statDisplayPanel.SetPlayerReferences(player);
            
        }
    }
}
