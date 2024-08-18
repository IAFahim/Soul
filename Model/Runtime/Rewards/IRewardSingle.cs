using Soul.Model.Runtime.Containers;

namespace Soul.Model.Runtime.Rewards
{
    public interface IRewardSingle<T, TV>
    {
        public Pair<T, TV> RewardSingleItem { get; }
    }
}