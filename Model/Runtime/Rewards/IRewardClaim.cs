namespace Soul.Model.Runtime.Rewards
{
    public interface IRewardClaim
    {
        public bool CanClaim();
        public void RewardClaim();
    }
}