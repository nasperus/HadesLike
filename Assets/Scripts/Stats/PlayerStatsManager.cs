using UnityEngine;
using System.Collections.Generic;
using Stats;

namespace Stats
{
    public class PlayerStatsManager : MonoBehaviour
    {
        
        [SerializeField] private StatCollection statCollection;

    
    private Dictionary<string, float> runtimeBaseValues = new Dictionary<string, float>();
    private Dictionary<string, float> statCaps = new Dictionary<string, float>
    {
        { "Haste", 90f },
        { "Mastery", 80f },
        { "Critical", 95f },
        { "Vitality", 300f },
        { "Armor", 65f },
        { "Mana", 300f },
        { "MovementSpeed", 15f },
    };

    private void Awake()
    {
        InitializeRuntimeBaseValues();
    }

  
    

    private void InitializeRuntimeBaseValues()
    {
        
        foreach (var stat in statCollection.GetAllStats())
        {
            runtimeBaseValues[stat.baseStats.statName] = stat.baseStats.baseValue;
        }
    }

   
    public void IncreaseBaseStat(string statName, float amount)
    {
        if (!runtimeBaseValues.ContainsKey(statName))
        {
            Debug.LogWarning($"Stat {statName} not found in runtime base values.");
            return;
        }
        var current = runtimeBaseValues[statName];
        var newValue = current + amount;

        if (statCaps.ContainsKey(statName))
        {
            var max = statCaps[statName];
            if (newValue > max)
            {
                newValue = max;
            }
            
        }

        runtimeBaseValues[statName] = newValue;
        UpdateStatBaseValue(statName);
    }

    // Call this to add temporary flat bonuses (like buffs)
    public void AddFlatBonus(string statName, float amount)
    {
        statCollection.AddFlatBonus(statName, amount);
    }

    // Reset bonuses on a stat
    public void ResetBonuses(string statName)
    {
        
        var stat = statCollection.GetRuntimeStat(statName);
        if (stat == null)
        {
            Debug.LogWarning($"Stat {statName} not found in StatCollection");
            return;
        }

        runtimeBaseValues[statName] = stat.baseStats.baseValue;
        

        // Reset flat/multiplier bonuses
        statCollection.ResetStatBonuses(statName);
        UpdateStatBaseValue(statName);
    }


    // Get the current value of a stat (base + flatBonus) * multiplier
    public float GetStatValue(string statName)
    {
        return statCollection.GetStatValue(statName);
    }

   
    private void UpdateStatBaseValue(string statName)
    {
       
        statCollection.ResetStatBonuses(statName);
        
        var stat = statCollection.GetRuntimeStat(statName);
        if (stat == null)
        {
            Debug.LogWarning($"Stat {statName} not found in StatCollection");
            return;
        }

        var originalBase = stat.baseStats.baseValue;
        var newBase = runtimeBaseValues[statName];
        var bonus = newBase - originalBase;

        stat.AddFlatBonus(bonus);

        //Debug.Log($"Updated base stat '{statName}' to runtime base {newBase}, applied flat bonus {bonus}");
    }
    }
}
