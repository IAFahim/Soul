using System;
using Soul.Model.Runtime.Limits;

namespace Soul.Model.Runtime.Levels
{
    [Serializable]
    public class Level : Limit
    {
        public bool IsLocked => IsZero();
        public event Action<int> OnLevelChange;

        public override int Current
        {
            get => base.Current;
            set
            {
                base.Current = value;
                InvokeOnLevelChange();
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


        public void InvokeOnLevelChange()
        {
            OnLevelChange?.Invoke(Current);
        }
    }
}