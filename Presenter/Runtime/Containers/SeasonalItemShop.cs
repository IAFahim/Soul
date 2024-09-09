using System.Collections.Generic;
using Pancake.Pools;
using Soul.Controller.Runtime.Lists;
using Soul.Controller.Runtime.LookUpTables;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.PoolAbles;
using TMPro;
using UnityEngine;

namespace Soul.Presenter.Runtime.Containers
{
    public class SeasonalItemShop : MonoBehaviour
    {
        [SerializeField] private TMP_Text seasonTitlePrefab;
        [SerializeField] private PriceLookUpTable priceLookUpTable;

        [SerializeField] private List<GameObject> instanceComponents;

        [SerializeField] private float itemHeight = 199;
        [SerializeField] private float itemColumns = 5;
        [SerializeField] private BuyComponent buyComponentPrefab;
        [SerializeField] private Sprite buyBackground;

        [SerializeField] private BuyComponent sellComponentPrefab;
        [SerializeField] private Sprite sellBackground;
        [SerializeField] private List<BuyComponent> sellComponents;
        [SerializeField] private RectTransform rectTransform;

        [SerializeField] private string text = "{0} Season";

        public void Setup(bool isBuy, AllowedItemLists allowedItemLists)
        {
            var title = seasonTitlePrefab.gameObject.Request<TMP_Text>(rectTransform.parent);

            title.text = string.Format(text, allowedItemLists.CurrentList.Title);
            float columns = Mathf.Ceil(allowedItemLists.CurrentList.Count / itemColumns);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, itemHeight * columns);
            if (isBuy) BuySetup(allowedItemLists.CurrentList);
            else SellSetup(allowedItemLists.CurrentList);

            title.transform.SetAsFirstSibling();

            // Clear(buyComponents);
            // Clear(sellComponents);
        }

        private void BuySetup(IList<Item> items)
        {
            foreach (var item in items)
            {
                var instance = buyComponentPrefab.gameObject.Request(transform);
                instanceComponents.Add(instance);

                priceLookUpTable.TryGetValue(item, out var price);
                var buyComponent = instance.GetComponent<BuyComponent>();
                buyComponent.Setup(item.icon, price, buyBackground);
            }
        }


        private void Clear<T>(IList<T> components) where T : PoolAbleComponent
        {
            if (components.Count == 0) return;
            foreach (var component in components) component.ReturnToPool();
        }

        private void SellSetup(IList<Item> items)
        {
            foreach (var item in items)
            {
                var sellComponent = sellComponentPrefab.gameObject.Request<BuyComponent>(transform);
                priceLookUpTable.TryGetValue(item, out var price);
                sellComponent.Setup(item.icon, price, sellBackground);
            }
        }
    }
}