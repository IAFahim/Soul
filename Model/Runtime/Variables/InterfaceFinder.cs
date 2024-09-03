using System;
using UnityEngine;

namespace Soul.Model.Runtime.Variables
{
    [Serializable]
    public struct InterfaceFinder<T>
    {
        [SerializeField] private bool isFound;

        public bool IsFound => isFound;
        public T Value { get; }

        public InterfaceFinder(T value) : this()
        {
            isFound = true;
            Value = value;
        }

        public InterfaceFinder(bool found, T value)
        {
            isFound = found;
            Value = value;
        }

        public bool TryGet<T>(Transform transform, ref InterfaceFinder<T> interfaceFinder)
        {
            if (transform.TryGetComponent(out T iInterface))
            {
                interfaceFinder = new InterfaceFinder<T>(iInterface);
                return true;
            }

            return false;
        }

        public static implicit operator InterfaceFinder<T>(T v)
        {
            return new InterfaceFinder<T>(v);
        }

        public static implicit operator T(InterfaceFinder<T> o)
        {
            return o.Value;
        }

        public static implicit operator bool(InterfaceFinder<T> o)
        {
            return o.IsFound;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}