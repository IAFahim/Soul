using System;
using UnityEngine;

namespace Soul.Model.Runtime.Reactives
{
    [Serializable]
    public struct Reactive<T>
    {
        [SerializeField] private T value;
        public event Action<T, T> OnChange;

        public T Value
        {
            get => value;
            set
            {
                T oldValue = this.value;
                this.value = value;
                OnChange?.Invoke(oldValue, value);
            }
        }
        
        public static implicit operator T(Reactive<T> reactive) => reactive.Value;
    }
}