using System;
using Soul.Model.Runtime.RequiredAndRewards.Rewards;
using UnityEngine;

namespace Soul.Presenter.Runtime.Vehicles.ClickRewardVehicles
{
    public class Tractor : Vehicle
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IRewardClaim>(out var rewardClaim))
            {
                if(rewardClaim.CanClaim) rewardClaim.RewardClaim();
            }
        }
    }
}