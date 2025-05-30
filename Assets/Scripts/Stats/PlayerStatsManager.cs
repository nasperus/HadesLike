using UnityEngine;
using System.Collections.Generic;
using Stats;

namespace Stats
{
    public class PlayerStatsManager : MonoBehaviour
    {
        
        [SerializeField] private StatCollection statCollection;

    
    private Dictionary<StatTypeEnum, float> runtimeBaseValues = new Dictionary<StatTypeEnum, float>();
    private Dictionary<StatTypeEnum, float> statCaps = new Dictionary<StatTypeEnum, float>
    {
        { StatTypeEnum.Haste, 90f },
        { StatTypeEnum.Mastery, 80f },
        { StatTypeEnum.Critical, 95f },
        { StatTypeEnum.Vitality, 200f },
        { StatTypeEnum.Armor, 65f },
        { StatTypeEnum.Mana, 200f },
        { StatTypeEnum.MovementSpeed, 15f },
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

   
    public void IncreaseBaseStat(StatTypeEnum statName, float amount)
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
    public void AddFlatBonus(StatTypeEnum statName, float amount)
    {
        statCollection.AddFlatBonus(statName, amount);
    }

    // Reset bonuses on a stat
    public void ResetBonuses(StatTypeEnum statName)
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
    public float GetStatValue(StatTypeEnum statName)
    {
        return statCollection.GetStatValue(statName);
    }
    
    public StatCollection GetStatCollection()
    {
        return statCollection;
    }

   
    private void UpdateStatBaseValue(StatTypeEnum statName)
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
