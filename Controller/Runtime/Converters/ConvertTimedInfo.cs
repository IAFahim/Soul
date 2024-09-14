using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Interfaces;
using UnityEngine;

namespace Soul.Controller.Runtime.Converters
{
    [Serializable]
    public class ConvertTimedInfo<T, TV> : ITimeRequirement
    {
        public T data;
        public TV ratio;
        [Range(0, 100)]public float xp = 1;
        
        public UnityTimeSpan timeRequired;
        public UnityTimeSpan RequiredTime => timeRequired;
        public float GetXpFrom(float amount) => xp * amount;
    }
}