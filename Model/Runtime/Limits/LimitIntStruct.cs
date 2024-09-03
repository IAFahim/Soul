using System;
using UnityEngine;

namespace Soul.Model.Runtime.Limits
{
    [Serializable]
    public struct LimitIntStruct
    {
        [BarAttribute.Bar] [SerializeField] private Vector2Int vector2Int;
        public int Current => vector2Int.x;

        public int Max => vector2Int.y;
        public bool IsMax => vector2Int.x >= vector2Int.y;

        public bool IsZero()
        {
            return Current == 0;
        }

        public LimitIntStruct(Vector2Int vector2Int)
        {
            this.vector2Int = vector2Int;
        }

        public LimitIntStruct(int current, int max)
        {
            vector2Int = new Vector2Int(current, max);
        }

        public LimitIntStruct(int max)
        {
            vector2Int = new Vector2Int(0, max);
        }

        public LimitIntStruct IfAdded(int value)
        {
            vector2Int.x += value;
            return this;
        }

        public static implicit operator int(LimitIntStruct limitInt)
        {
            return limitInt.Current;
        }

        public static implicit operator float(LimitIntStruct limitInt)
        {
            return (float)limitInt.Current / limitInt.Max;
        }

        public static LimitIntStruct operator +(LimitIntStruct limitInt, int value)
        {
            limitInt.vector2Int.x += value;
            return limitInt;
        }

        public static LimitIntStruct operator -(LimitIntStruct limitInt, int value)
        {
            limitInt.vector2Int.x -= value;
            return limitInt;
        }

        public static LimitIntStruct operator *(LimitIntStruct limitInt, int value)
        {
            limitInt.vector2Int.x *= value;
            return limitInt;
        }

        public static LimitIntStruct operator /(LimitIntStruct limitInt, int value)
        {
            limitInt.vector2Int.x /= value;
            return limitInt;
        }
    }
}