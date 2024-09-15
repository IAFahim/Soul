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

namespace Soul.Presenter.Runtime.Containers
{
    public class SeasonalItemShop : MonoBehaviour
    {
        [SerializeField] private TMP_Text seasonTitlePrefab;

        [SerializeField] private float itemHeight = 199;
        [SerializeField] private float itemColumns = 5;
        [SerializeField] private BuyComponent buyComponentPrefab;
        [SerializeField] private Sprite buyBackground;

        [SerializeField] private BuyComponent sellComponentPrefab;
        [SerializeField] private Sprite sellBackground;
        [SerializeField] private RectTransform rectTransform;

        [SerializeField] private string text = "{0} Season";

        private PlayerFarmReference _playerFarmReference;
        private PriceLookUpTable _priceLookUpTable;
        private Dictionary<Item, BuyComponent> buyComponents = new();
        private Dictionary<Item, BuyComponent> sellComponents = new();

        public void Setup(bool isBuy, AllowedItemLists allowedItemLists, PlayerFarmReference playerFarmReference,
            PriceLookUpTable priceLookUpTable)
        {
            _playerFarmReference = playerFarmReference;
            _priceLookUpTable = priceLookUpTable;
            var title = seasonTitlePrefab.gameObject.Request<TMP_Text>(rectTransform.parent);

            title.text = string.Format(text, allowedItemLists.CurrentList.Title);
            float columns = Mathf.Ceil(allowedItemLists.CurrentList.Count / itemColumns);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, itemHeight * columns);
            if (isBuy) BuySetup(allowedItemLists.CurrentList, playerFarmReference, priceLookUpTable);
            else SellSetup(allowedItemLists.CurrentList);

            title.transform.SetAsFirstSibling();

            // Clear(buyComponents);
            // Clear(sellComponents);
        }

        private void BuySetup(IList<Item> items, PlayerFarmReference playerFarmReference,
            PriceLookUpTable priceLookUpTable)
        {
            Clear(sellComponents);
            playerFarmReference.inventory.OnItemChanged += InventoryOnItemChanged;
            foreach (var item in items)
            {
                var buyComponent = buyComponentPrefab.gameObject.Request<BuyComponent>(transform);
                buyComponents.Add(item, buyComponent);
                SetBuyComponent(playerFarmReference, priceLookUpTable, item, buyComponent);
            }
        }

        private void SetBuyComponent(PlayerFarmReference playerFarmReference, PriceLookUpTable priceLookUpTable,
            Item item, BuyComponent buyComponent
        )
        {
            priceLookUpTable.TryGetValue(item, out var price);
            playerFarmReference.inventory.TryGetValue(item, out var count);
            playerFarmReference.inventory.TryGetMaxValue(item, out var max);
            buyComponent.Setup(item.icon, price, buyBackground, count, max);
        }

        private void InventoryOnItemChanged(InventoryChangeEventArgs<Item, int> changeEventArgs)
        {
            if (buyComponents.TryGetValue(changeEventArgs.Key, out var buyComponent))
            {
                SetBuyComponent(_playerFarmReference, _priceLookUpTable, changeEventArgs.Key, buyComponent);
            }
        }


        private void Clear<T>(Dictionary<Item, T> components) where T : PoolAbleComponent
        {
            foreach (var component in components)
            {
                component.Value.ReturnToPool();
            }

            components.Clear();
        }

        private void SellSetup(IList<Item> items)
        {
        }
    }
}