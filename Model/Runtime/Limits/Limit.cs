using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Model.Runtime.Limits
{
    [Serializable]
    public class Limit
    {
        [FormerlySerializedAs("level")] [BarAttribute.Bar] public Vector2Int vector2Int;

        public int Current
        {
            get => vector2Int.x;
            set => vector2Int.x = value;
        }
        
        public int Max => vector2Int.y;
        public bool IsMax => vector2Int.x >= vector2Int.y;
        public bool IsZero() => Current == 0;
        
        public static implicit operator int(Limit limit) => limit.Current;
    }
}