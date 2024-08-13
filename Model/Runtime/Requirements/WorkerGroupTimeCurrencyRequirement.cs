using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Workers;

namespace Soul.Model.Runtime.Requirements
{
    [Serializable]
    public struct WorkerGroupTimeCurrencyRequirement<T, TV>
    {
        public WorkerGroup workerGroup;
        public UnityTimeSpan time;
        public Pair<T, TV>[] currencyRequirements;
        public Pair<T, TV>[] items;
    }
}