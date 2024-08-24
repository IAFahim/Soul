using Soul.Model.Runtime.Containers;
using UnityEngine;

namespace Soul.Model.Runtime.RequiredAndRewards
{
    public class RequiredAndRewardScriptableObject<TRequirement, TReward> : ScriptableObject
    {
        [SerializeField] protected Pair<TRequirement, TReward>[] requiredAndRewards;

        public Pair<TRequirement, TReward> GetRequiredAndRewards(int index) => requiredAndRewards[index];
        public TRequirement GetRequirement(int index) => requiredAndRewards[index].First;
        public TReward GetReward(int index) => requiredAndRewards[index].Second;

        public int Length => requiredAndRewards.Length;
    }
}