using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Peoples.Workers;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public struct ProductionRequirement
    {
        public bool isProducing;
        public Item productionItem;
        [FormerlySerializedAs("productionWorker")] public WorkerType productionWorkerType;
        public UnityTimeSpan productionTimeStartTimeSpan;
        public UnityTimeSpan productionTimeReduction;
    }
}