using System;
using Soul.Model.Runtime.Limits;

namespace Soul.Model.Runtime.Levels
{
    [Serializable]
    public class Level : Limit
    {
        public bool IsLocked => IsZero();
        public event Action<int, int> OnLevelChange;

        public override int Current
        {
            get => base.Current;
            set
            {
                var old = base.Current;
                base.Current = value;
                OnLevelChange?.Invoke(old, Current);
            }
        }

        public bool IncreaseLevelByOne()
        {
            if (IsMax) return false;
            Current = currentAndMax.x + 1;
            return true;
        }

        public static implicit operator int(Level level)
        {
            return level.Current;
        }
    }
}