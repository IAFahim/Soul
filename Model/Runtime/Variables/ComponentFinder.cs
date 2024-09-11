using System;
using UnityEngine;

namespace Soul.Model.Runtime.Variables
{
    [Serializable]
    public struct ComponentFinder<T>
    {
        [SerializeField] private bool isFound;

        public bool IsFound => isFound;
        public T Value { get; }

        public ComponentFinder(T value) : this()
        {
            isFound = true;
            Value = value;
        }

        public ComponentFinder(bool found, T value)
        {
            isFound = found;
            Value = value;
        }

        public bool TryGet<TComponent>(Transform transform, ref ComponentFinder<TComponent> componentFinder)
        {
            if (transform.TryGetComponent(out TComponent iInterface))
            {
                componentFinder = new ComponentFinder<TComponent>(iInterface);
                return true;
            }

            return false;
        }

        public static implicit operator ComponentFinder<T>(T v)
        {
            return new ComponentFinder<T>(v);
        }

        public static implicit operator T(ComponentFinder<T> o)
        {
            return o.Value;
        }

        public static implicit operator bool(ComponentFinder<T> o)
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