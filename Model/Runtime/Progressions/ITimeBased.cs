using QuickEye.Utility;

namespace Soul.Model.Runtime.Progressions
{
    public interface ITimeBased
    {
        public UnityDateTime StartedAt { get; set; }
        public UnityTimeSpan Discount { get; set; }
        public UnityTimeSpan GetTimeAfterDiscount(UnityTimeSpan fullTime);
        public UnityDateTime EndsAt(UnityTimeSpan fullTime);
        public UnityTimeSpan Remaining(UnityTimeSpan fullTime);

        public float ProgressRatio(UnityTimeSpan fullTime);

        public bool IsOver(UnityTimeSpan fullTime);
    }
}