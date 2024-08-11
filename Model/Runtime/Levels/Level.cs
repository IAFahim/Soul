using System;
using UnityEngine;

namespace Soul.Model.Runtime.Levels
{
    [Serializable]
    public class Level
    {
        [BarAttribute.Bar] public Vector2Int level;

        public bool IsLocked => Mathf.Approximately(CurrentLevel, 0);
        public int CurrentLevel => level.x;
        public int MaxLevel => level.y;
        public bool AtMaxLevel => level.x >= level.y;

        public bool IncreaseLevel()
        {
            if (AtMaxLevel) return false;
            level.x++;
            return true;
        }
        
        public static implicit operator int(Level level) => level.CurrentLevel;
    }
}