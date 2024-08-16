using System;
using QuickEye.Utility;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class RequirementBasicWithTime
    {
        public UnityTimeSpan timeSpanStartTime;
        public UnityTimeSpan timeSpawnReductionTime;
    }
}