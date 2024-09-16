using System;
using System.Collections.Generic;
using Pancake.Pools;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Lists;
using Soul.Controller.Runtime.LookUpTables;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Inventories;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.PoolAbles;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Presenter.Runtime.Containers
{
    public class SeasonalItemShop : MonoBehaviour
    {
        [SerializeField] private TMP_Text seasonTitlePrefab;

        [SerializeField] private float itemHeight = 199;
        [SerializeField] private float itemColumns = 5;

        [FormerlySerializedAs("buyComponentPrefab")] [SerializeField]
        private BuySellComponent buySellComponentPrefab;

        [FormerlySerializedAs("sellComponentPrefab")] [SerializeField]
        private BuySellComponent sellSellComponentPrefab;

        [SerializeField] private Sprite sellBackground;
        [SerializeField] private RectTransform rectTransform;

        [SerializeField] private string text = "{0} Season";

        private PlayerFarmReference _playerFarmReference;
        private PriceLookUpTable _priceLookUpTable;
        private TMP_Text title;
        private Dictionary<Item, BuySellComponent> buySellComponents = new();

        public void Setup(bool isBuy, AllowedItemLists allowedItemLists, Sprite background,
            PlayerFarmReference playerFarmReference, PriceLookUpTable priceLookUpTable)
        {
            _playerFarmReference = playerFarmReference;
            _priceLookUpTable = priceLookUpTable;
            if (title == null) title = seasonTitlePrefab.gameObject.Request<TMP_Text>(rectTransform.parent);
            title.text = string.Format(text, allowedItemLists.CurrentList.Title);
            float columns = Mathf.Ceil(allowedItemLists.CurrentList.Count / itemColumns);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, itemHeight * columns);

            Clear(buySellComponents);
            Set(isBuy, allowedItemLists.CurrentList, background, playerFarmReference, priceLookUpTable);

            title.transform.SetAsFirstSibling();
        }

        private void Set(bool isBuy, IList<Item> items, Sprite background,
            PlayerFarmReference playerFarmReference, PriceLookUpTable priceLookUpTable
        )
        {
            playerFarmReference.inventory.OnItemChanged -= InventoryOnBuyItemChanged;
            playerFarmReference.inventory.OnItemChanged += InventoryOnBuyItemChanged;
            foreach (var item in items)
            {
                var buySellComponent = buySellComponentPrefab.gameObject.Request<BuySellComponent>(transform);
                buySellComponents.Add(item, buySellComponent);
                buySellComponent.Setup(isBuy, playerFarmReference, priceLookUpTable, item, background);
            }
        }

        private void InventoryOnBuyItemChanged(InventoryChangeEventArgs<Item, int> changeEventArgs)
        {
            if (buySellComponents.TryGetValue(changeEventArgs.Key, out var buyComponent))
            {
                buyComponent.Set(changeEventArgs.Key, changeEventArgs.NewAmount);
            }
        }

        public void OnDisable()
        {
            _playerFarmReference.inventory.OnItemChanged -= InventoryOnBuyItemChanged;
        }


        private void Clear<T>(Dictionary<Item, T> components) where T : PoolAbleComponent
        {
            foreach (var component in components)
            {
                component.Value.ReturnToPool();
            }

            components.Clear();
        }
    }
}