using System.Collections.Generic;
using Pancake;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Model.Runtime.Bags
{
    public class AdsRewardBag : ScriptableObject
    {
        public Bag<Item> bag;
        public Dictionary<Item, int> rewardDictionary;
    }
}