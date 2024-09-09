using Alchemy.Inspector;
using Alchemy.Serialization;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Sound;
using Pancake.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Views
{
    [AlchemySerialize]
    public partial class SettingView : View
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Color onColor = Color.green;
        [SerializeField] private Color offColor = Color.black;
        [SerializeField] private Button musicButton;
        [SerializeField] private TMP_Text musicButtonText;

        [SerializeField] private Button soundButton;
        [SerializeField] private TMP_Text soundButtonText;
        [SerializeField] private Button bugAndFeedbackButton;

        [HorizontalLine, SerializeField, PopupPickup]
        private string bugAndFeedbackPopupKey;

        protected override UniTask Initialize()
        {
            closeButton.onClick.AddListener(OnCloseButtonPressed);
            bugAndFeedbackButton.onClick.AddListener(OnBugAndFeedbackButtonPressed);
            musicButton.onClick.AddListener(OnMusicButtonPressed);
            SetButtonBasedOnVolume(musicButton, musicButtonText, AudioManager.MusicVolume, onColor, offColor);
            soundButton.onClick.AddListener(OnSoundButtonPressed);
            SetButtonBasedOnVolume(soundButton, soundButtonText, AudioManager.SfxVolume, onColor, offColor);
            return UniTask.CompletedTask;
        }

        private void SetButtonBasedOnVolume(Button button, TMP_Text text, float value, Color on, Color off)
        {
            if (Mathf.Approximately(value, 0))
            {
                button.image.color = offColor;
                text.text = "Off";
                return;
            }

            button.image.color = onColor;
            text.text = "On";
        }

        private void OnSoundButtonPressed()
        {
            var volumeToggle = AudioManager.SfxVolume;
            AudioManager.SfxVolume = Mathf.Approximately(volumeToggle, 0) ? 1 : 0;
            SetButtonBasedOnVolume(soundButton, soundButtonText, AudioManager.SfxVolume, onColor, offColor);
        }

        private void OnMusicButtonPressed()
        {
            var volumeToggle = AudioManager.MusicVolume;
            AudioManager.MusicVolume = Mathf.Approximately(volumeToggle, 0) ? 1 : 0;
            SetButtonBasedOnVolume(musicButton, musicButtonText, AudioManager.MusicVolume, onColor, offColor);
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