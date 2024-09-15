using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using Pancake.UI;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Lists;
using Soul.Controller.Runtime.LookUpTables;
using Soul.Model.Runtime.Containers;
using Soul.Presenter.Runtime.Containers;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Views
{
    public partial class ShopView : View
    {
        [SerializeField] private PlayerFarmReference playerFarmReference;
        [SerializeField] private PriceLookUpTable priceLookUpTable;

        [SerializeField] private Button buyButton;
        [SerializeField] private Button sellButton;
        [SerializeField] private SeasonalItemShop seasonalItemShop;
        [SerializeField] private Button closeButton;
        [SerializeField] private RectTransform spawnRectTransform;
        [SerializeField] private Pair<AllowedItemLists, AllowedItemLists>[] allowedItemBuySellLists;
        [SerializeField] private int productIndex;

        protected override UniTask Initialize()
        {
            closeButton.onClick.AddListener(OnCloseButtonPressed);
            buyButton.onClick.AddListener(Buy);
            sellButton.onClick.AddListener(Sell);
            Setup();
            return UniTask.CompletedTask;
        }

        private void Buy()
        {
            PlayerPrefs.SetInt("isBuy", 1);
            Setup();
        }

        private void Sell()
        {
            PlayerPrefs.SetInt("isBuy", 0);
            Setup();
        }


        [Button]
        private void Setup()
        {
            var seasonalItemShopInstance = seasonalItemShop.gameObject.Request<SeasonalItemShop>(spawnRectTransform);
            bool isBuy = PlayerPrefs.GetInt("isBuy", 0) == 1;
            AllowedItemLists allowedItemLists =
                isBuy ? allowedItemBuySellLists[productIndex].First : allowedItemBuySellLists[productIndex].Second;
            seasonalItemShopInstance.Setup(isBuy, allowedItemLists, playerFarmReference, priceLookUpTable);
        }

        private void OnCloseButtonPressed()
        {
            PopupHelper.Close(Transform);
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonPressed);
        }
    }
}