using System;
using UnityEngine;

namespace Soul.Model.Runtime.Reactives
{
    [Serializable]
    public class Reactive<T>
    {
        [SerializeField] protected T value;
        public event Action<T, T> OnChange;

        public virtual T Value
        {
            get => value;
            set
            {
                T oldValue = this.value;
                this.value = value;
                OnChange?.Invoke(oldValue, this.value);
            }
        }

        public static implicit operator T(Reactive<T> reactive)
        {
            return reactive.Value;
        }
    }
}