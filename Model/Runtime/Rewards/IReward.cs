using System;

namespace Soul.Model.Runtime.Rewards
{
    public interface IReward<out T>
    {
        public T Reward { get; }
    }
}