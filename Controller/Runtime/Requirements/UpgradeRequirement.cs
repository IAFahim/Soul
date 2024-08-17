using System;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class UpgradeRequirement : RecordWorkerWithTime
    {
        public bool isUpgrading;
        public int toLevel = 1;
    }
}