using System;
using Soul.Model.Runtime.Rewards;
using UnityEngine;

namespace Soul.Controller.Runtime.Rewards
{
    [Serializable]
    public class RewardClaimer<T>
    {
        public Transform target;
        [SerializeReference] public IReward<T> reward;
        [SerializeReference] public IRewardClaim rewardClaim;

        public void Setup(Transform targetTransform, IReward<T> rewardProvider, IRewardClaim rewardClaimReference)
        {
            target = targetTransform;
            reward = rewardProvider;
            rewardClaim = rewardClaimReference;
        }

        public bool Claim()
        {
            if (!rewardClaim.CanClaim) return false;
            rewardClaim.RewardClaim();
            return true;
        }
    }
}