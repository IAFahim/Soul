using System;

namespace Soul.Model.Runtime.Reactives
{
    [Serializable]
    public struct Reactive<T>
    {
        public T value;
        public event Action<T, T> OnChange;

        public T Value
        {
            get => value;
            set
            {
                T oldValue = value;
                this.value = value;
                OnChange?.Invoke(oldValue, value);
            }
        }
    }
}