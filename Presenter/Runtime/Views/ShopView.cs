using Cysharp.Threading.Tasks;
using Pancake.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Views
{
    public partial class ShopView : View
    {
        [SerializeField] private Button closeButton;
        private readonly Vector2 _maxTabHeigh = new(250, 107);
        private readonly Vector2 _minTabHeigh = new(250, 82);

        protected override UniTask Initialize()
        {
            closeButton.onClick.AddListener(OnCloseButtonPressed);
            return UniTask.CompletedTask;
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