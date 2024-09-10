using System;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProgressBar = UnityProgressBar.ProgressBar;

namespace Soul.Presenter.Runtime.Panels
{
    public class UpgradeUnlockPanel : MonoBehaviour
    {
        public TMP_Text upgradeTitle;
        public Color upgradeHighlightColor = new Color(0.01f, 0.63f, 0.02f, 1);
        public string upgradeTitleFormat = "Upgrade {0} To <color={1}>Level {2}</color>";
        public string unlockTitleFormat = "Unlock {0}";
        public string maxLevelTitleFormat = "{0} is already at max level";

        public ProgressBar upgradeLevelProgressBar;
        public TMP_Text upgradeTimeTitle;
        public TMPFormat upgradeTimeFormat;

        public TMP_Text startButtonTitle;
        public TMPFormat startCoinRequirementFormat;

        public Button startButton;
        public Button closeButton;

        private Action _onStartButtonPressed;
        private Action _onCancelButtonPressed;
        

        public void Show(RectTransform rectTransform, Transform currentSelectedTransform, Level levelReference,
            Action onStartButtonPressed, Action onCancelButtonPressed)
        {
            startButton.onClick.RemoveAllListeners();
            gameObject.SetActive(true);
            transform.SetParent(rectTransform, false);
            _onStartButtonPressed = onStartButtonPressed;
            _onCancelButtonPressed = onCancelButtonPressed;
            closeButton.onClick.AddListener(OnCancelButtonPressed);
            if (levelReference.IsLocked) UnlockPrompt(currentSelectedTransform, onStartButtonPressed);
            else if (levelReference.IsMax) MaxLevelPrompt(onStartButtonPressed);
            else UpgradePrompt(currentSelectedTransform, onStartButtonPressed);
        }

        private void OnCancelButtonPressed()
        {
            Hide();
            _onCancelButtonPressed?.Invoke();
        }

        private void UpgradePrompt(Transform currentSelectedTransform, Action onStartButtonPressed)
        {
            // Implement the logic to show the upgrade prompt
            var upgradeRef = currentSelectedTransform.GetComponent<IUpgrade>();
            if (upgradeRef.IsUpgrading)
            {
                startButton.interactable = false;
            }
            else
            {
                startButton.interactable = true;
                startButtonTitle.text = "Upgrade";
                startButton.onClick.AddListener(onStartButtonPressed.Invoke);
            }
        }
        
        public void TimerSetup(float time)
        {
            // Implement the logic to setup the timer
        }

        private void MaxLevelPrompt(Action onStartButtonPressed)
        {
            // Implement the logic to show the max level prompt
        }

        private void UnlockPrompt(Transform currentSelectedTransform, Action onStartButtonPressed)
        {
            // Implement the logic to show the unlock prompt
        }
        

        public void Hide()
        {
            gameObject.SetActive(false);
            transform.SetParent(null);
        }
    }
}