using Soul.Model.Runtime.Claims;

namespace Soul.Model.Runtime.Rewards
{
    public interface IRewardClaim : IClaimAble
    {
        public void RewardClaim();
    }
}