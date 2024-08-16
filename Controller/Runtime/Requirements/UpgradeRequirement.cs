using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Peoples.Workers;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public struct UpgradeRequirement
    {
        public bool isUpgrading;
        [FormerlySerializedAs("upgradeWorker")] public WorkerType upgradeWorkerType;
        public UnityTimeSpan upgradeTimeStartTimeSpan;
        public UnityTimeSpan upgradeTimeReduction;
    }
}