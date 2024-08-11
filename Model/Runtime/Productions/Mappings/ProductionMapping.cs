using System;
using Pancake;
using Soul.Model.Runtime.Modifiers;

namespace Soul.Model.Runtime.Productions.Mappings
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