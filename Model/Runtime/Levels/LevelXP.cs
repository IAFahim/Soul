using System;
using Pancake.Common;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.SaveAndLoad;
using UnityEngine;

namespace Soul.Model.Runtime.Levels
{
    [Serializable]
    public class LevelXp : Level, ISaveAble
    {
        [SerializeField, Pancake.Guid] private string guid;
        [SerializeField] private float baseXp = 100f;
        [SerializeField] private float xpMultiplier = 1.5f;

        [SerializeField] private int xp;
        public float XpProgress { get; private set; }
        private int _xpToNextLevel;

        public event Action<int, int> OnXpChange;

        public override int Current
        {
            get => base.Current;
            set
            {
                if (value > Max)
                {
                    value = Max;
                }

                base.Current = value;
                CalculateXpToNextLevel();
            }
        }

        public int XpToNextLevel => _xpToNextLevel;
        public int Xp => xp;

        public void AddXp(int amount, bool save = true)
        {
            if (IsMax) return;
            var oldXp = Xp;
            xp = Xp + amount;
            CalculateXpProgress();

            while (Xp >= XpToNextLevel && !IsMax)
            {
                xp = Xp - XpToNextLevel;
                IncreaseLevelByOne();
            }

            OnXpChange?.Invoke(oldXp, Xp);
            if (save) Save();
        }

        private void CalculateXpToNextLevel()
        {
            if (IsMax)
            {
                _xpToNextLevel = int.MaxValue;
                XpProgress = 1f;
                return;
            }

            _xpToNextLevel = Mathf.RoundToInt(baseXp * Mathf.Pow(xpMultiplier, Current - 1));
            CalculateXpProgress();
        }

        private void CalculateXpProgress()
        {
            XpProgress = IsMax ? 1f : Mathf.Clamp01((float)Xp / XpToNextLevel);
        }

        public void ResetXp()
        {
            var oldXp = Xp;
            xp = 0;
            CalculateXpProgress();
            OnXpChange?.Invoke(oldXp, Xp);
        }

        private int GetTotalXpForLevel(int level)
        {
            if (level <= 1) return 0;
            return Mathf.RoundToInt(baseXp * (Mathf.Pow(xpMultiplier, level - 1) - 1) / (xpMultiplier - 1));
        }

        public override string ToString()
        {
            return $"Level: {Current}/{Max}, XP: {Xp}/{XpToNextLevel}, Progress: {XpProgress:P2}";
        }

        public void Save(string key = default)
        {
            key ??= guid;
            Pair<int, int> data = new Pair<int, int>(Current, Xp);
            Data.Save(key, data);
        }

        private void Initialize(int startLevel = 1, int startXp = 0)
        {
            Current = startLevel;
            xp = startXp;
            CalculateXpToNextLevel();
            CalculateXpProgress();
        }

        public void Load(string key = default)
        {
            key ??= guid;
            Pair<int, int> data = Data.Load(key, new Pair<int, int>(1, 0));
            Initialize(data.First, data.Second);
        }

        public Pair<int, int> ToPair() => new(Current, Xp);
        public void FromPair(Pair<int, int> pair) => Initialize(pair.First, pair.Second);
    }
}