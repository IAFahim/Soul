using System;
using UnityEngine;

namespace Soul.Model.Runtime.Workers
{
    [Serializable]
    public struct WorkerGroup
    {
        public WorkerType workerType;
        [SerializeField] public int count;
        public event Action<WorkerGroup, int> OnAdd;
        public event Action<WorkerGroup, int> OnRemove;

        public void Add(int value)
        {
            count += value;
            OnAdd?.Invoke(this, value);
        }

        public void Subtract(int value)
        {
            count -= value;
            OnAdd?.Invoke(this, -value);
        }
    }
}