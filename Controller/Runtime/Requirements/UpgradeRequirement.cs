using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Workers;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public struct UpgradeRequirement
    {
        public bool isUpgrading;
        public Worker upgradeWorker;
        public UnityTimeSpan upgradeTimeStartTimeSpan;
        public UnityTimeSpan upgradeTimeReduction;
    }
}