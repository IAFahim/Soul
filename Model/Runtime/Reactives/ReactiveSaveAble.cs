using System;
using Pancake;
using Pancake.Common;
using Soul.Model.Runtime.SaveAndLoad;

namespace Soul.Model.Runtime.Reactives
{
    [Serializable]
    public class ReactiveSaveAble<T> : Reactive<T>, ISaveAble
    {
        [Guid] public string guid;
        public bool saveNextChange = true;
        
        public override T Value
        {
            get => base.Value;
            set
            {
                base.Value = value;
                if (saveNextChange) Save(guid);
                saveNextChange = true;
            }
        }

        public void Save(string key=default)
        {
            key ??= guid;
            Data.Save(key, Value);
        }

        public void Load(string key=default)
        {
            key ??= guid;
            value = Data.Load<T>(key);
        }
        
    }
}