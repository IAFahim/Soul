using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Workers;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public struct ProductionRequirement
    {
        public bool isProducing;
        public Item productionItem;
        public Worker productionWorker;
        public UnityTimeSpan productionTimeStartTimeSpan;
        public UnityTimeSpan productionTimeReduction;
    }
}