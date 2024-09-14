using System.Collections.Generic;
using Pancake;
using Soul.Controller.Runtime.InfoPanels;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.RequiredAndRewards.Rewards;
using UnityEngine;

namespace Soul.Presenter.Runtime.Vehicles.ClickRewardVehicles
{
    public class Boat : Vehicle, IInfoPanelReference, IReward<Dictionary<Item, int>>, IRewardClaim
    {
        public Bag<Item> bag;
        [SerializeField] public int rewardAmount;

        private void Start()
        {
            bag.Initialize();
        }

        public override void OnSelected(RaycastHit selfRayCastHit)
        {
            
        }


        public Dictionary<Item, int> Reward
        {
            get
            {
                var rewardDictionary = new Dictionary<Item, int>();
                for (int i = 0; i < rewardAmount; i++)
                {
                    var reward = bag.Pick();
                    if (!rewardDictionary.TryAdd(reward, 1)) rewardDictionary[reward]++;
                }
                return rewardDictionary;
            }
        }

        public bool CanClaim => true;

        public void RewardClaim()
        {
            
        }
    }
}