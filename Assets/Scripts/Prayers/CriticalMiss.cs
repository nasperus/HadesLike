
using UnityEngine;

namespace Prayers
{
    public static class CriticalMiss
    {
        public static bool ShouldMiss( int  rarityLevel)
        {
            var roll = Random.Range(0f, 100f);
            return roll <= GetMissChance(rarityLevel);
        }
        
        public static float GetMissChance(int  rarityLevel)
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
