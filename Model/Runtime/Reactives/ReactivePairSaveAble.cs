using System;
using Pancake.Common;
using Soul.Model.Runtime.SaveAndLoad;

namespace Soul.Model.Runtime.Reactives
{
    [Serializable]
    public class ReactivePairSaveAble<T, TV> : ReactivePair<T, TV>, ISaveAble
    {
        public bool saveNextChange = true;
        public int age;
        public override TV Value
        {
            get => base.Value;
            set
            {
                base.Value = value;
                if (saveNextChange) Save();
                saveNextChange = true;
            }
        }

        public void Save(string key = default)
        {
            key ??= GetDefaultSaveKey();
            Data.Save(key, Value);
        }

        public void Load(string key = default)
        {
            key ??= GetDefaultSaveKey();
            pair.Value = Data.Load<TV>(key);
        }

        protected virtual string GetDefaultSaveKey()
        {
            return $"ReactivePairSaveAble_{typeof(T).Name}_{typeof(TV).Name}";
        }
    }
}