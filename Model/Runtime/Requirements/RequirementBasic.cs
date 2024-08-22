using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Containers;

namespace Soul.Model.Runtime.Requirements
{
    [Serializable]
    public struct RequirementBasic<T, TV>
    {
        public int workerCount;
        public UnityTimeSpan fullTime;
        public Pair<T, TV> currency;
        public Pair<T, TV>[] items;
    }
}