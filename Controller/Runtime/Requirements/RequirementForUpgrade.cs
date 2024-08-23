using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public struct RequirementForUpgrade
    {
        public int workerCount;
        public UnityTimeSpan fullTime;
        public Pair<Currency, int> currency;
        public Pair<Item, int>[] items;
    }
}