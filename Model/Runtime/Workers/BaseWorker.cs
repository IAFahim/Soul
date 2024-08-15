using System;
using UnityEngine;

namespace Soul.Model.Runtime.Workers
{
    public abstract class BaseWorker : IWorker
    {
        [SerializeField] private int count;
        public event Action<IWorker, int, int> OnChange;

        public int Count
        {
            get => count;
            set
            {
                int oldValue = count;
                count = value;
                OnChange?.Invoke(this, oldValue, count);
            }
        }
    }
}