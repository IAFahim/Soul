using System;
using Soul.Model.Runtime.Containers;
using UnityEngine;

namespace Soul.Model.Runtime.Reactives
{
    [Serializable]
    public struct ReactivePair<T, TV>
    {
        [SerializeField] private Pair<T, TV> pair;
        
        public event Action<T, TV, TV> OnChange;

        public Pair<T, TV> Pair1
        {
            get => pair;
            set
            {
                TV oldValue = pair.Value;
                pair = value;
                OnChange?.Invoke(pair.Key, oldValue, pair.Value);
            }
        }
    }
}