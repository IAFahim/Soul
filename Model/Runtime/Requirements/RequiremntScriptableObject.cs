using UnityEngine;

namespace Soul.Model.Runtime.Requirements
{
    public class RequiremntScriptableObject<T> : ScriptableObject
    {
        [SerializeField] protected T[] requirements;

        public T GetRequirement(int index) => requirements[index];
    }
}