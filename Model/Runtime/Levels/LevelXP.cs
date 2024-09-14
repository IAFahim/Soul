using System;
using UnityEngine;
using Soul.Model.Runtime.Limits;

namespace Soul.Model.Runtime.Levels
{
    [Serializable]
    public class LevelXp : Level
    {
        [SerializeField] private float baseXp = 100f; // Starting XP for level 2
        [SerializeField] private float xpMultiplier = 1.5f; // How much XP increases each level

        public int Xp { get; private set; }
        public int XpToNextLevel { get; private set; }
        public float XpProgress { get; private set; } // New property for XP progress

        public event Action<int> OnXpChange;

        public override int Current 
        { 
            get => base.Current; 
            set
            {
                base.Current = value;
                CalculateXpToNextLevel();
            }
        }

        public void AddXp(int amount)
        {
            Xp += amount;
            CalculateXpProgress(); // Update progress after XP changes
            OnXpChange?.Invoke(Xp);

            while (Xp >= XpToNextLevel && !IsMax)
            {
                Xp -= XpToNextLevel;
                IncreaseLevelByOne();
            }
        }

        private void CalculateXpToNextLevel()
        {
            if (IsMax) 
            {
                XpToNextLevel = 0; // No more levels
                XpProgress = 1f; // Full progress at max level
                return;
            }

            // Formula: XP = BaseXP * Multiplier^(Level - 1) 
            XpToNextLevel = Mathf.RoundToInt(baseXp * Mathf.Pow(xpMultiplier, Current)); 
            CalculateXpProgress(); // Update progress after XP to next level changes
        }

        private void CalculateXpProgress()
        {
            if (XpToNextLevel == 0) // Handle cases where XpToNextLevel is 0 (e.g., max level)
            {
                XpProgress = 1f;
            }
            else
            {
                XpProgress = Mathf.Clamp01((float)Xp / XpToNextLevel); 
            }
        }

        public void ResetXp()
        {
            Xp = 0;
            CalculateXpProgress(); // Update progress after reset
            OnXpChange?.Invoke(Xp);
        }
    }
}