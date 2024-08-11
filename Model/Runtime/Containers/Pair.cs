using System;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Containers
{
    [Serializable]
    public struct Pair<T, TV>
    {
        [SerializeField] private T key;

        [SerializeField] private TV value;

        public Pair(T key, TV value)
        {
            this.key = key;
            this.value = value;
        }

        public Pair(Pair<T, TV> source)
        {
            key = source.key;
            value = source.value;
        }

        public T Key
        {
            get => key;
            set => key = value;
        }

        public TV Value
        {
            get => value;
            set => this.value = value;
        }
        

        private String GetIdentity()
        {
            return "[" + key + " , " + value + "]";
        }

        public override string ToString()
        {
            return GetIdentity();
        }

        public override int GetHashCode()
        {
            return key.GetHashCode() ^ value.GetHashCode();
        }
    }
}