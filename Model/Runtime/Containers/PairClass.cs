using System;
using UnityEngine;

namespace Soul.Model.Runtime.Containers
{
    [Serializable]
    public class PairClass<T, TV>
    {
        [SerializeField]
        private T first;

        [SerializeField]
        private TV second;

        public PairClass(T first, TV second)
        {
            this.first = first;
            this.second = second;
        }

        public PairClass(Pair<T, TV> source)
        {
            first = source.First;
            second = source.Second;
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
            set => second = value;
        }

        public TV Multiplier
        {
            get => second;
            set => second = value;
        }

        public TV Second
        {
            get => second;
            set => second = value;
        }


        private string GetIdentity()
        {
            return "[" + first + " , " + second + "]";
        }

        public override string ToString()
        {
            return GetIdentity();
        }

        public override int GetHashCode()
        {
            return First.GetHashCode() ^ Second.GetHashCode();
        }

        public static T ToT(Pair<T, TV> pair)
        {
            return pair.First;
        }

        public static TV ToTV(Pair<T, TV> pair)
        {
            return pair.Second;
        }
    }
}