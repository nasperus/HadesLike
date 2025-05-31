using System;
using UnityEngine;

namespace Player
{
    public class PlayerSpawner : MonoBehaviour
    {
       [SerializeField] private GameObject playerPrefab;

       private void Awake()
       {
          
           Instantiate(playerPrefab, transform.position, Quaternion.identity,transform);
          
       }

    }
}
