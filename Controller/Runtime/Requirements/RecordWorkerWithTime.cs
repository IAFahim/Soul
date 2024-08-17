using System;
using QuickEye.Utility;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class RecordWorkerWithTime: RecordWorker
    {
        public UnityDateTime startTime;
        public UnityTimeSpan reductionTime;
    }
}