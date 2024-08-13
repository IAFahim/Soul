using UnityEngine;

namespace Soul.Model.Runtime.Requirements
{
    public class RequirementOfWorkerGroupTimeCurrencyForLevels<T, TV> : ScriptableObject
    {
        public WorkerGroupTimeCurrencyRequirement<T, TV>[] requirements;

        public WorkerGroupTimeCurrencyRequirement<T, TV> GetRequirement(int level) => requirements[level];
    }
}