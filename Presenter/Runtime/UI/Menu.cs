using Alchemy.Inspector;
using Pancake;
using Pancake.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button buttonSetting;
        [SerializeField] private Button buttonShop;
        [SerializeField] private Button buttonBugAndFeedback;
        [HorizontalLine, SerializeField, PopupPickup] private string settingPopupKey;
        [HorizontalLine, SerializeField, PopupPickup] private string shopPopupKey;
        [HorizontalLine, SerializeField, PopupPickup] private string bugAndFeedbackPopupKey;

        private void OnEnable()
        {
            buttonSetting.onClick.AddListener(OpenSetting);
            buttonShop.onClick.AddListener(OpenShop);
            buttonBugAndFeedback.onClick.AddListener(OpenBugAndFeedback);
        }

        private void OpenBugAndFeedback()
        {
            MainUIContainer.In.GetMain<PopupContainer>().Push(bugAndFeedbackPopupKey, true);
        }

        private void OpenShop()
        {
            MainUIContainer.In.GetMain<PopupContainer>().Push(shopPopupKey, true);
        }

        private void OpenSetting()
        {
            MainUIContainer.In.GetMain<PopupContainer>().Push(settingPopupKey, true);
        }
        
        private void OnDisable()
        {
            buttonSetting.onClick.RemoveListener(OpenSetting);
            buttonShop.onClick.RemoveListener(OpenShop);
        }
    }
}