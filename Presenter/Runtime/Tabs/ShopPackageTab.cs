using System;
using System.Collections.Generic;
using Pancake.Common;
using Soul.Model.Runtime;
using Soul.Model.Runtime.UIs;
using Soul.Model.Runtime.UIs.Packages;
using Soul.Presenter.Runtime.Visuals;
using UnityEngine;

namespace Soul.Presenter.Runtime.Tabs
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