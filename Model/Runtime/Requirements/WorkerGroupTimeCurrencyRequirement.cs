using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Containers;
using UnityEngine.Serialization;

namespace Soul.Model.Runtime.Requirements
{
    [Serializable]
    public struct WorkerGroupTimeCurrencyRequirement<T, TV>
    {
        public int workerCount;
        [FormerlySerializedAs("time")] public UnityTimeSpan fullTime;
        [FormerlySerializedAs("currencyRequirement")] public Pair<T, TV> currency;
        public Pair<T, TV>[] items;
    }
}