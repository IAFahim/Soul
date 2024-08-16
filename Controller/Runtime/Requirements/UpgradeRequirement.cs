using System;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class UpgradeRequirement : RequirementBasicWithTime
    {
        public bool isUpgrading;
        public int toLevel = 1;
    }
}