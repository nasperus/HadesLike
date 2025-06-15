
using UnityEngine;

namespace Prayers
{
    public static class CriticalMiss
    {

        public static bool? CanMiss { get; set; } = null;
        public static bool? ShouldMiss( int  rarityLevel)
        {
            if (CanMiss != true) return null;
            var roll = Random.Range(0f, 100f);
            return roll <= GetMissChance(rarityLevel);
        }

        private static float GetMissChance(int  rarityLevel)
        {
            return rarityLevel switch
            {
                1 => 5f,   
                2 => 10f, 
                3 => 15f,  
                4 => 20f,  
                _ => 0f    
            };
        }
    }
}
