using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Workers;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public struct ProductionRequirement
    {
        public bool isProducing;
        public Item productionItem;
        [FormerlySerializedAs("productionWorkerses")] [FormerlySerializedAs("productionWorker")] public Workers productionWorkers;
        public UnityTimeSpan productionTimeStartTimeSpan;
        public UnityTimeSpan productionTimeReduction;
    }
}