using Ability_System.Enum;
using UnityEngine;

namespace Prayers
{
    public static class CriticalMiss
    {
        public static bool ShouldMiss(this BoonRarity rarity)
        {
            var roll = Random.Range(0f, 100f);
            return roll <= GetMissChance(rarity);
        }
        
        public static float GetMissChance(this BoonRarity rarity)
        {
            return rarity switch
            {
                BoonRarity.Common => 5f,
                BoonRarity.Rare => 10f,
                BoonRarity.Epic => 15f,
                BoonRarity.Legendary => 20f,
                _ => 0f
            };
        }
    }
}
