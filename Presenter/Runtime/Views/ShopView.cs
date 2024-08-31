using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using Pancake.UI;
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
            seasonalItemShopInstance.Setup(true);
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