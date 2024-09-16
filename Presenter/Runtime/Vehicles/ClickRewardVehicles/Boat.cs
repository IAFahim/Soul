using System.Collections.Generic;
using Alchemy.Inspector;
using Pancake;
using Pancake.UI;
using Soul.Controller.Runtime.InfoPanels;
using Soul.Model.Runtime.Bags;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.RequiredAndRewards.Rewards;
using UnityEngine;

namespace Soul.Presenter.Runtime.Vehicles.ClickRewardVehicles
{
    public class Boat : Vehicle, IInfoPanelReference, IReward<Dictionary<Item, int>>
    {
        public AdsRewardBag rewardBag;
        [SerializeField] public int rewardAmount;

        [HorizontalLine, SerializeField, PopupPickup]
        private string adsRewardPopupKey;

        private void Start()
        {
            rewardBag.bag.Initialize();
        }

        public override void OnSelected(RaycastHit selfRayCastHit)
        {
            rewardBag.rewardDictionary = Reward;
            MainUIContainer.In.GetMain<PopupContainer>().Push(adsRewardPopupKey, true);
        }


        public Dictionary<Item, int> Reward
        {
            get
            {
                var rewardDictionary = new Dictionary<Item, int>();
                for (int i = 0; i < rewardAmount; i++)
                {
                    var reward = rewardBag.bag.Pick();
                    if (!rewardDictionary.TryAdd(reward, 1)) rewardDictionary[reward]++;
                }
                return rewardDictionary;
            }
        }
    }
}