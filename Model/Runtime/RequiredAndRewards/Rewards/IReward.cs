namespace Soul.Model.Runtime.RequiredAndRewards.Rewards
{
    public interface IReward<out T>
    {
        public T Reward { get; }
    }
}