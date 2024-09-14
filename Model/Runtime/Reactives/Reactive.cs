using System;
using UnityEngine;

namespace Soul.Model.Runtime.Reactives
{
    [Serializable]
    public class Reactive<T>
    {
        [SerializeField] private T value;
        public event Action<T, T> OnChange;

        public T Value
        {
            get => value;
            set
            {
                var oldValue = this.value;
                this.value = value;
                OnChange?.Invoke(oldValue, value);
            }
        }

        public static implicit operator T(Reactive<T> reactive)
        {
            return reactive.Value;
        }
    }
}