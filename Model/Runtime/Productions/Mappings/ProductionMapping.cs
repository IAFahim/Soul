using System;
using _Root.Scripts.Model.Runtime.Modifiers;
using Pancake;

namespace _Root.Scripts.Model.Runtime.Productions.Mappings
{
    [Serializable]
    public struct ProductionMapping<T>
    {
        public T required;
        public Optional<T> fee;
        public Modifier modifier;
        public Optional<T> reward;
    }
}