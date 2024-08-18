using System;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class RecordUpgrade : RecordWorkerWithTime
    {
        public bool isUpgrading;
        public int toLevel = 1;
    }
}