using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Interfaces;

namespace Soul.Controller.Runtime.Converters
{
    [Serializable]
    public class ConvertTimedInfo<T, TV> : ITimeRequirement
    {
        public T data;
        public TV ratio;
        public UnityTimeSpan timeRequired;
        public UnityTimeSpan RequiredTime => timeRequired;
    }
}