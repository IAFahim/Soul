using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Peoples.Workers;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public struct RequirementForUpgrade
    {
        public Pair<WorkerType, int> worker;
        public UnityTimeSpan fullTime;
        public int coin;
        public int gem;
        public Pair<Item, int>[] items;
    }
}