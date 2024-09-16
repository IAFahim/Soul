using System;
using UnityEngine;

namespace Soul.Model.Runtime.Limits
{
    [Serializable]
    public class Limit
    {
        [BarAttribute.Bar] [SerializeField] protected Vector2Int currentAndMax;

        public Vector2Int SetWithoutNotify(int current, int max)
        {
            currentAndMax.x = current;
            currentAndMax.y = max;
            return currentAndMax;
        }

        public virtual int Current
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
        public float Progress => (float)Current / Max;

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