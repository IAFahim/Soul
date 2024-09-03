using Soul.Model.Runtime.Containers;
using UnityEngine;

namespace Soul.Model.Runtime.RequiredAndRewards
{
    public class RequiredAndRewardScriptableObject<TRequirement, TReward> : ScriptableObject
    {
        [SerializeField] protected Pair<TRequirement, TReward>[] requiredAndRewards;

        public int Length => requiredAndRewards.Length;

        public Pair<TRequirement, TReward> GetRequiredAndRewards(int index)
        {
            return requiredAndRewards[index];
        }

        public TRequirement GetRequirement(int index)
        {
            return requiredAndRewards[index].First;
        }

        public TReward GetReward(int index)
        {
            return requiredAndRewards[index].Second;
        }
    }
}