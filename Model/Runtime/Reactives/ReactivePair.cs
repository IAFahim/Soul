using System;
using Soul.Model.Runtime.Containers;
using UnityEngine;

namespace Soul.Model.Runtime.Reactives
{
    [Serializable]
    public class ReactivePair<T, TV>
    {
        [SerializeField] private Pair<T, TV> pair;

        public event Action<T, TV, TV> OnChange;

        public T Key => pair.Key;

        public TV Value
        {
            get => pair.Value;
            set
            {
                var oldValue = pair.Value;
                pair.Value = value;
                OnChange?.Invoke(pair.Key, oldValue, value);
            }
        }


        public static implicit operator T(ReactivePair<T, TV> pair)
        {
            return pair.pair.Key;
        }

        public static implicit operator TV(ReactivePair<T, TV> pair)
        {
            return pair.pair.Value;
        }
    }
}