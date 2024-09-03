using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Model.Runtime.Limits
{
    [Serializable]
    public class Limit
    {
        [FormerlySerializedAs("vector2Int")] [BarAttribute.Bar]
        public Vector2Int currentAndMax;

        public int Current
        {
            get => currentAndMax.x;
            set => currentAndMax.x = value;
        }

        public int Max
        {
            get => currentAndMax.y;
            set => currentAndMax.y = value;
        }

        public bool IsMax => currentAndMax.x >= currentAndMax.y;

        public bool IsZero()
        {
            return Current == 0;
        }

        public static implicit operator int(Limit limit)
        {
            return limit.Current;
        }
    }
}