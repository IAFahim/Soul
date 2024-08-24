using Soul.Controller.Runtime.Requirements;
using Soul.Controller.Runtime.Rewards;
using Soul.Model.Runtime.RequiredAndRewards;
using UnityEngine;

namespace Soul.Controller.Runtime.RequiresAndRewards
{
    [CreateAssetMenu(fileName = "Required And Reward", menuName = "Soul/RequiredAndReward/Production")]
    public class RequiredAndRewardForProductions : RequiredAndRewardScriptableObject<RequirementForProduction, RewardForProduction>
    {
    }
}