using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Progressions;

namespace Soul.Model.Runtime.Records
{
    [Serializable]
    public struct RecordTime : ITimeBased
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
        
        public UnityTimeSpan GetTimeAfterDiscount(UnityTimeSpan fullTime) => fullTime - Discount;
        public UnityDateTime EndsAt(UnityTimeSpan fullTime) => StartedAt + GetTimeAfterDiscount(fullTime);
        public UnityTimeSpan Remaining(UnityTimeSpan fullTime) => EndsAt(fullTime) - DateTime.UtcNow;

        public float Progress(UnityTimeSpan fullTime) => 1 - (float)Remaining(fullTime).TotalSeconds /
            (float)GetTimeAfterDiscount(fullTime).TotalSeconds;

        public bool IsOver(UnityTimeSpan fullTime) => Remaining(fullTime) <= TimeSpan.Zero;
    }
}