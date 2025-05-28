using UnityEngine;
using System.Collections.Generic;

namespace Stats
{
    public class StatCollection : MonoBehaviour
    {
        [SerializeField] private List<RuntimeStats> stats = new List<RuntimeStats>();

        public float GetStatValue(string statName)
        {
            foreach (var stat in stats)
            {
                if (stat.baseStats.statName == statName)
                {
                    return stat.Value;
                }
            }
            return 0f;
        }

        public void AddFlatBonus(string statName, float bonus)
        {
            foreach (var stat in stats)
            {
                if (stat.baseStats.statName == statName)
                {
                    stat.AddFlatBonus(bonus);
                    return;
                }
            }
        }

        public void AddMultiplier(string statName, float multiplier)
        {
            foreach (var stat in stats)
            {
                if (stat.baseStats.statName == statName)
                {
                    stat.AddMultiplier(multiplier);
                    return;
                }
            }
        }

        public void ResetStatBonuses(string statName)
        {
            foreach (var stat in stats)
            {
                if (stat.baseStats.statName == statName)
                {
                    stat.ResetBonuses();
                    return;
                }
            }
        }
        
        public RuntimeStats GetRuntimeStat(string statName)
        {
            foreach (var stat in stats)
            {
                if (stat.baseStats.statName == statName)
                    return stat;
            }
            return null;
        }

        public IEnumerable<RuntimeStats> GetAllStats()
        {
            return stats;
        }
        
        
    }
}
