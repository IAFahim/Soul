using System;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Variables
{
    [Serializable]
    public struct InterfaceFinder<T>
    {
        [SerializeField] private bool isFound;
        private T _value;
        
        public bool IsFound => isFound;
        public T Value => _value;

        public InterfaceFinder(T value) : this()
        {
            isFound = true;
            _value = value;
        }

        public InterfaceFinder(bool found, T value)
        {
            isFound = found;
            _value = value;
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

        public static implicit operator InterfaceFinder<T>(T v) { return new InterfaceFinder<T>(v); }

        public static implicit operator T(InterfaceFinder<T> o) { return o.Value; }

        public static implicit operator bool(InterfaceFinder<T> o) { return o.IsFound; }
        public override int GetHashCode() { return Value.GetHashCode(); }
        public override string ToString() { return Value.ToString(); }
        
    }
}