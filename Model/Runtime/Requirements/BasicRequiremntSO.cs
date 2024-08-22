using UnityEngine;

namespace Soul.Model.Runtime.Requirements
{
    public class BasicRequiremntSO<T, TV> : ScriptableObject
    {
        [SerializeField] protected RequirementBasic<T, TV>[] requirements;

        public RequirementBasic<T, TV> GetRequirement(int index) => requirements[index];
    }
}