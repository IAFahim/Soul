using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Progressions;

namespace Soul.Model.Runtime.Records
{
    [Serializable]
    public class RecordTime : ITimeBased
    {
        public UnityDateTime startedAt;
        public UnityTimeSpan discount;

        public UnityDateTime StartedAt
        {
            get => startedAt;
            set => startedAt = value;
        }

        public UnityTimeSpan Discount
        {
            get => discount;
            set => discount = value;
        }

        public UnityTimeSpan GetTimeAfterDiscount(UnityTimeSpan fullTime)
        {
            return fullTime - Discount;
        }

        public UnityDateTime EndsAt(UnityTimeSpan fullTime)
        {
            return StartedAt + GetTimeAfterDiscount(fullTime);
        }

        public UnityTimeSpan Remaining(UnityTimeSpan fullTime)
        {
            return EndsAt(fullTime) - DateTime.UtcNow;
        }

        public float ProgressRatio(UnityTimeSpan fullTime) => 1 - (float)Remaining(fullTime).TotalSeconds /
            (float)GetTimeAfterDiscount(fullTime).TotalSeconds;

        public bool IsOver(UnityTimeSpan fullTime)
        {
            return Remaining(fullTime) <= TimeSpan.Zero;
        }
    }
}