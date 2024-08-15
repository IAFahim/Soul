using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Containers;

namespace Soul.Model.Runtime.Requirements
{
    [Serializable]
    public struct WorkerGroupTimeCurrencyRequirement<T, TV>
    {
        public int workerCount;
        public UnityTimeSpan time;
        public Pair<T, TV>[] currencyRequirements;
        public Pair<T, TV>[] items;
    }
}