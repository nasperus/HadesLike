using UnityEngine;

namespace Room_Generation
{
    [System.Serializable]
    public class RoomData
    {
        public GameObject prefab;
        public Vector3 size = new Vector3(15f, 0f, 15f);
    }
}
