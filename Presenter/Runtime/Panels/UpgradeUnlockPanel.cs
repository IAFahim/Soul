using System;
using LitMotion;
using LitMotion.Extensions;
using Pancake.Common;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.RequiresAndRewards;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
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
        public RectTransform selfRectTransform;
        public float duration = 0.5f;
        public Ease ease = Ease.OutQuad;
        public Vector3 anchorStartPosition = new(-600, 0, 0);
        public Vector3 anchorEndPosition = new(-500, 0, 0);
        public MotionHandle anchorMotionHandle;

        public Vector3 scaleStart = new(0.1f, 0.1f, 0.1f);
        public Vector3 scaleEnd = new(1, 1, 1);
        public MotionHandle scaleMotionHandle;

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

        private PlayerInventoryReference _playerInventoryReference;
        private EventShowItemRequired _eventShowItemRequired;
        private Action _onStartButtonPressed;
        private ILevel _levelReference;
        private IRequirementForUpgradeScriptableReference _requirementForUpgradeReference;
        private IUpgrade _upgradeReference;

        public Pair<Currency, int> CurrencyRequirement(int level) =>
            _requirementForUpgradeReference.RequirementForUpgrades.GetRequirement(level).currency;

        public Pair<Item, int>[] ItemsRequirement(int level) =>
            _requirementForUpgradeReference.RequirementForUpgrades.GetRequirement(level).items;

        public void Show(RectTransform parentRect, Transform currentSelectedTransform,
            PlayerInventoryReference playerInventoryReference, EventShowItemRequired eventShowItemRequired,
            ITitle titleReference, ILevel levelReference,
            Action onStartButtonPressed)
        {
            _onStartButtonPressed = onStartButtonPressed;
            _levelReference = levelReference;
            _playerInventoryReference = playerInventoryReference;
            _eventShowItemRequired = eventShowItemRequired;
            SetPanel(parentRect, currentSelectedTransform, titleReference, levelReference, onStartButtonPressed);
        }

        private void SetPanel(RectTransform parentRect, Transform currentSelectedTransform, ITitle titleReference,
            ILevel levelReference, Action onStartButtonPressed)
        {
            startButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCancelButtonPressed);
            SetParent(parentRect);
            SetTitle(titleReference, levelReference);
            SetProgressBar(levelReference.Level);
            SetPanelButtonInfo(parentRect, currentSelectedTransform, levelReference, onStartButtonPressed);
        }

        private void SetPanelButtonInfo(RectTransform parentRect, Transform currentSelectedTransform,
            ILevel levelReference,
            Action onStartButtonPressed)
        {
            if (levelReference.Level.IsMax) MaxLevelPrompt(onStartButtonPressed);
            else if (levelReference.Level.IsLocked) UnlockPrompt(currentSelectedTransform, onStartButtonPressed);
            else UpgradePrompt(currentSelectedTransform, levelReference.Level);
        }

        private void SetParent(RectTransform parentRect, bool playAnimation = true)
        {
            selfRectTransform.SetParent(parentRect);

            if (anchorMotionHandle.IsActive()) anchorMotionHandle.Complete();
            anchorMotionHandle = LMotion.Create(anchorStartPosition, anchorEndPosition, duration).WithEase(ease)
                .BindToAnchoredPosition3D(selfRectTransform);

            if (scaleMotionHandle.IsActive()) scaleMotionHandle.Complete();
            scaleMotionHandle = LMotion.Create(scaleStart, scaleEnd, duration).WithEase(ease)
                .BindToLocalScale(selfRectTransform);
            gameObject.SetActive(true);
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

        private void UpgradePrompt(Transform currentSelectedTransform, Level level)
        {
            _upgradeReference = currentSelectedTransform.GetComponent<IUpgrade>();
            SetItemRequirement(currentSelectedTransform, level);
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

        private void SetItemRequirement(Transform currentSelectedTransform, Level level)
        {
            _requirementForUpgradeReference =
                currentSelectedTransform.GetComponent<IRequirementForUpgradeScriptableReference>();
            _eventShowItemRequired.Trigger(ItemsRequirement(level));
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

        private void SetProgressBar(Level level)
        {
            upgradeLevelProgressBar.Value = (level.Current / (float)level.Max);
        }


        private void OnCancelButtonPressed()
        {
            _eventShowItemRequired.Trigger(null);
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            transform.SetParent(null);
        }
    }
}