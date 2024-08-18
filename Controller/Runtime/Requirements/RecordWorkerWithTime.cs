using System;
using QuickEye.Utility;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class RecordWorkerWithTime : RecordWorker
    {
        public UnityDateTime startTime;
        public UnityTimeSpan reductionTime;

        public UnityTimeSpan DiscountedTime(UnityTimeSpan fullTime) => fullTime - reductionTime;
        public UnityDateTime EndTime(UnityTimeSpan fullTime) => startTime + DiscountedTime(fullTime);
        public UnityTimeSpan TimeRemaining(UnityTimeSpan fullTime) => EndTime(fullTime) - DateTime.UtcNow;

        public float Progress(UnityTimeSpan fullTime) => 1 - (float)TimeRemaining(fullTime).TotalSeconds /
            (float)DiscountedTime(fullTime).TotalSeconds;
        public bool IsCompleted(UnityTimeSpan fullTime) => TimeRemaining(fullTime) <= TimeSpan.Zero;
    }
}