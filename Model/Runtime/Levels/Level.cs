using System;
using Soul.Model.Runtime.Limits;

namespace Soul.Model.Runtime.Levels
{
    [Serializable]
    public class Level : Limit
    {
        public bool IsLocked => IsZero();

        public bool IncreaseLevel()
        {
            if (IsMax) return false;
            currentAndMax.x++;
            return true;
        }

        public static implicit operator int(Level level)
        {
            return level.Current;
        }
    }
}