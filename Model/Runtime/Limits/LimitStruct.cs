using System;
using UnityEngine;

namespace Soul.Model.Runtime.Limits
{
    [Serializable]
    public struct LimitStruct
    {
        [BarAttribute.Bar, SerializeField] private Vector2Int vector2Int;
        public int Current => vector2Int.x;

        public int Max => vector2Int.y;
        public bool IsMax => vector2Int.x >= vector2Int.y;
        public bool IsZero() => Current == 0;

        public static implicit operator int(LimitStruct limit) => limit.Current;
    }
}