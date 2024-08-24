using Soul.Model.Runtime.Claims;

namespace Soul.Model.Runtime.RequiredAndRewards.Rewards
{
    public interface IRewardClaim : IClaimAble
    {
        public void RewardClaim();
    }
}