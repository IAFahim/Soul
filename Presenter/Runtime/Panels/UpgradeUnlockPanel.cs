using System;
using Pancake.Common;
using Soul.Controller.Runtime.RequiresAndRewards;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Interfaces;
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
        public string unlockTitleFormat = "Unlock <color={0}>{1}</color>";
        public string maxLevelTitleFormat = "{0} at {1} level is already at max";

        public ProgressBar upgradeLevelProgressBar;
        public TMP_Text upgradeTimeTitle;
        public TMPFormat upgradeTimeFormat;

        public TMP_Text startButtonTitle;
        public TMPFormat startCoinRequirementFormat;

        public Button startButton;
        public Button closeButton;

        private Action _onStartButtonPressed;
        private ILevel _levelReference;
        private IRequirementForUpgradeScriptableReference _requirementForUpgrade;
        private IUpgrade _upgradeReference;

        public void Show(RectTransform parentRect, Transform currentSelectedTransform,
            ITitle titleReference, ILevel levelReference,
            IRequirementForUpgradeScriptableReference requirementForUpgrade,
            Action onStartButtonPressed
        )
        {
            startButton.onClick.RemoveAllListeners();
            _onStartButtonPressed = onStartButtonPressed;
            _levelReference = levelReference;
            _requirementForUpgrade = requirementForUpgrade;

            closeButton.onClick.AddListener(OnCancelButtonPressed);
            SetParent(parentRect);
            SetTitle(titleReference, levelReference);
            SetPanelButtonInfo(parentRect, currentSelectedTransform, levelReference, onStartButtonPressed);
        }

        private void SetPanelButtonInfo(RectTransform parentRect, Transform currentSelectedTransform,
            ILevel levelReference,
            Action onStartButtonPressed)
        {
            if (levelReference.Level.IsLocked) UnlockPrompt(currentSelectedTransform, onStartButtonPressed);
            else if (levelReference.Level.IsMax) MaxLevelPrompt(onStartButtonPressed);
            else UpgradePrompt(parentRect, currentSelectedTransform, onStartButtonPressed);
        }

        private void SetParent(RectTransform parentRect)
        {
            gameObject.SetActive(true);
            transform.SetParent(parentRect);
        }

        private void SetTitle(ITitle titleReference, ILevel levelReference)
        {
            if (levelReference.Level.IsMax)
            {
                upgradeTitle.text = string.Format(maxLevelTitleFormat, titleReference.Title, levelReference.Level);
            }
            else if (levelReference.Level.IsLocked)
            {
                upgradeTitle.text = string.Format(unlockTitleFormat, upgradeHighlightColor.ToHtmlStringRGB(),
                    titleReference.Title);
            }
            else
            {
                upgradeTitle.text = string.Format(upgradeTitleFormat,
                    titleReference, upgradeHighlightColor.ToHtmlStringRGB(), levelReference.Level.Current + 1
                );
            }
        }

        private void UpgradePrompt(RectTransform parentRect, Transform currentSelectedTransform,
            Action onStartButtonPressed)
        {
            _upgradeReference = currentSelectedTransform.GetComponent<IUpgrade>();
            if (_upgradeReference != null)
            {
                if (_upgradeReference.IsUpgrading)
                {
                    startButton.interactable = false;
                }
                else
                {
                    startButton.interactable = true;
                    startButtonTitle.text = "Upgrade";
                    startButton.onClick.AddListener(Upgrade);
                }
            }
            else Hide();
        }

        private void Upgrade()
        {
            _upgradeReference.Upgrade();
            _onStartButtonPressed?.Invoke();
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


        private void OnCancelButtonPressed()
        {
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            transform.SetParent(null);
        }
    }
}