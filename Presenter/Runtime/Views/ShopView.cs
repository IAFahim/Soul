using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using Pancake.UI;
using Soul.Controller.Runtime.Lists;
using Soul.Model.Runtime.Containers;
using Soul.Presenter.Runtime.Containers;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Views
{
    public partial class ShopView : View
    {
        [SerializeField] private SeasonalItemShop seasonalItemShop;
        [SerializeField] private Button closeButton;
        [SerializeField] private RectTransform spawnRectTransform;
        [SerializeField] private Pair<AllowedItemLists, AllowedItemLists>[] allowedItemBuySellLists;
        [SerializeField] private bool isBuy;
        [SerializeField] private int productIndex;

        protected override UniTask Initialize()
        {
            closeButton.onClick.AddListener(OnCloseButtonPressed);
            Setup();
            return UniTask.CompletedTask;
        }

        [Button]
        private void Setup()
        {
            var seasonalItemShopInstance = seasonalItemShop.gameObject.Request<SeasonalItemShop>(spawnRectTransform);
            var allowedItemLists =
                isBuy ? allowedItemBuySellLists[productIndex].First : allowedItemBuySellLists[productIndex].Second;
            seasonalItemShopInstance.Setup(isBuy, allowedItemLists);
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