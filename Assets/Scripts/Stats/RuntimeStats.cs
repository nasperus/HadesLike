namespace Stats
{
    [System.Serializable]
    public class RuntimeStats
    {
        public BaseStats baseStats;
        public float flatBonus;
        public float multiplier;
        
        public float Value => (baseStats.baseValue + flatBonus) * multiplier;
        

        public void AddFlatBonus(float bonus)
        {
            flatBonus += bonus;
        }
        
        public void AddMultiplier(float multi)
        {
            multiplier *= multi;
        }
        
        public void ResetBonuses()
        {
            flatBonus = 0;
            multiplier = 1;
        }
        
 
    }
}
