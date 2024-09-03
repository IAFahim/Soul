using UnityEngine;

namespace Soul.Model.Runtime.RequiredAndRewards.Requirements
{
    public class RequirementScriptableObject<T> : ScriptableObject
    {
        [SerializeField] protected T[] requirements;

        public T GetRequirement(int index)
        {
            return requirements[index];
        }
    }
}