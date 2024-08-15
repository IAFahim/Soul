using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Workers;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public struct UpgradeRequirement
    {
        public bool isUpgrading;
        [FormerlySerializedAs("upgradeWorkerses")] [FormerlySerializedAs("upgradeWorker")] public Workers upgradeWorkers;
        public UnityTimeSpan upgradeTimeStartTimeSpan;
        public UnityTimeSpan upgradeTimeReduction;
    }
}