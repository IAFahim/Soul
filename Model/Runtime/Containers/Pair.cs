using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Model.Runtime.Containers
{
    [Serializable]
    public struct Pair<T, TV>
    {
        [FormerlySerializedAs("key")] [SerializeField] private T first;

        [FormerlySerializedAs("value")] [SerializeField] private TV second;

        public Pair(T first, TV second)
        {
            this.first = first;
            this.second = second;
        }

        public Pair(Pair<T, TV> source)
        {
            first = source.first;
            second = source.second;
        }

        public T Key
        {
            get => first;
            set => first = value;
        }

        public T First
        {
            get => first;
            set => first = value;
        }

        public TV Value
        {
            get => second;
            set => this.second = value;
        }
        
        public TV Multiplier
        {
            get => second;
            set => this.second = value;
        }

        public TV Second
        {
            get => second;
            set => this.second = value;
        }


        private String GetIdentity()
        {
            return "[" + first + " , " + second + "]";
        }

        public override string ToString()
        {
            return GetIdentity();
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() ^ second.GetHashCode();
        }

        public static implicit operator T(Pair<T, TV> pair) => pair.first;
        public static implicit operator TV(Pair<T, TV> pair) => pair.second;
    }
}