using Alchemy.Inspector;
using Alchemy.Serialization;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Views
{
    [AlchemySerialize]
    public partial class SettingView : View
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button soundButton;
        [SerializeField] private Button bugAndFeedbackButton;
        
        [HorizontalLine, SerializeField, PopupPickup] private string bugAndFeedbackPopupKey;
        protected override UniTask Initialize()
        {
            closeButton.onClick.AddListener(OnCloseButtonPressed);
            bugAndFeedbackButton.onClick.AddListener(OnBugAndFeedbackButtonPressed);
            return UniTask.CompletedTask;
        }

        private void OnBugAndFeedbackButtonPressed()
        {
            BugAndFeedbackButtonPressed().Forget();
        }

        private void OnCloseButtonPressed()
        { 
            PopupHelper.Close(Transform);
        }
        
        private async UniTask BugAndFeedbackButtonPressed()
        {
            await PopupHelper.Close(Transform);
            await MainUIContainer.In.GetMain<PopupContainer>().Push(bugAndFeedbackPopupKey, true);
        }

        public void OnDisable()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonPressed);
        }
    }
}