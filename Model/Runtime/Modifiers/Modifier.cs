using System;
using UnityEngine;

namespace Soul.Model.Runtime.Modifiers
{
    [Serializable]
    public struct Modifier : IEquatable<Modifier>
    {
        [SerializeField] private float baseValue;
        [SerializeField] private float multiplier;
        [SerializeField] private float additive;

        public float Value => (baseValue * multiplier) + additive;

        public Modifier(float baseValue, float multiplier = 1f, float additive = 0f)
        {
            this.baseValue = baseValue;
            this.multiplier = multiplier;
            this.additive = additive;
        }

        public float BaseValue
        {
            get => baseValue;
            set => baseValue = value;
        }

        public float Multiplier
        {
            get => multiplier;
            set => multiplier = value;
        }

        public float Additive
        {
            get => additive;
            set => additive = value;
        }

        public void Set(float baseVal, float mul, float add)
        {
            baseValue = baseVal;
            multiplier = mul;
            additive = add;
        }

        /// <summary>
        /// Applies a chance-based multiplier to the current Value.
        /// </summary>
        /// <param name="chance">The probability of applying the bonus multiplier. If >= 1, it's treated as a guaranteed multiplier.</param>
        /// <param name="bonusMultiplier">The multiplier to apply if the chance check succeeds.</param>
        /// <returns>The result of Value multiplied by the determined multiplier.</returns>
        public float ApplyChanceMultiplier(float chance, float bonusMultiplier)
        {
            float chanceMultiplier = chance >= 1f ? chance : (UnityEngine.Random.value < chance ? bonusMultiplier : 1f);
            return Value * chanceMultiplier;
        }

        public static implicit operator float(Modifier modifier) => modifier.Value;
        public static implicit operator Modifier(float value) => new Modifier(value);

        public bool Equals(Modifier other) =>
            baseValue.Equals(other.baseValue) && multiplier.Equals(other.multiplier) && additive.Equals(other.additive);

        public override bool Equals(object obj) => obj is Modifier other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(baseValue, multiplier, additive);

        public static bool operator ==(Modifier left, Modifier right) => left.Equals(right);
        public static bool operator !=(Modifier left, Modifier right) => !(left == right);
    }
}