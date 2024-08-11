using System;
using System.Collections.Generic;
using _Root.Scripts.Model.Runtime;
using _Root.Scripts.Model.Runtime.UIs.Packages;
using Pancake.Common;
using Pancake.Game.UI;
using UnityEngine;

namespace _Root.Scripts.Presenter.Runtime.Tabs
{
    public class ShopPackageTab : MonoBehaviour
    {
        [SerializeField] private PackageItemVisual packageItemVisualPrefab;
        [SerializeField] private Transform content;

        public void Initialize(List<PackagePackData> packagesData, Func<EShopRewardType, ShopItemRewardConfig> getShopItemReward)
        {
            content.RemoveAllChildren();
            foreach (var data in packagesData)
            {
                var visual = Instantiate(packageItemVisualPrefab, content);
                visual.Setup(data, getShopItemReward);
            }
        }
    }
}