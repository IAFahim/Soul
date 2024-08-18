using System;
using QuickEye.Utility;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class RecordWorkerWithTime : RecordWorker
    {
        public UnityDateTime startTime;
        public UnityTimeSpan reductionTime;

        public UnityTimeSpan RequiredTime(UnityTimeSpan requiredTime) => requiredTime - reductionTime;
        public UnityDateTime EndTime(UnityTimeSpan requiredTime) => startTime + RequiredTime(requiredTime);
        public UnityTimeSpan TimeRemaining(UnityTimeSpan requiredTime) => EndTime(requiredTime) - DateTime.UtcNow;

        public float Progress(UnityTimeSpan requiredTime) => 1 - (float)TimeRemaining(requiredTime).TotalSeconds /
            (float)RequiredTime(requiredTime).TotalSeconds;
        public bool IsCompleted(UnityTimeSpan requiredTime) => TimeRemaining(requiredTime) <= TimeSpan.Zero;
    }
}