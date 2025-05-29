using UnityEngine;


namespace Stats
{
    [CreateAssetMenu(menuName = "Stats/BaseStat")]
    public class BaseStats : ScriptableObject
    {
        public StatTypeEnum statName;
        public float baseValue;

    }
}


    

