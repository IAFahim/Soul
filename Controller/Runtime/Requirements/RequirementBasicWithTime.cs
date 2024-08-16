using System;
using QuickEye.Utility;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class RequirementBasicWithTime: RequirementBasic
    {
        public UnityDateTime startTime;
        public UnityTimeSpan reductionTime;
    }
}