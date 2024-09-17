using System;
using LitMotion;
using LitMotion.Extensions;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.RequiresAndRewards;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.UpgradeAndUnlock;
using Soul.Model.Runtime.UpgradeAndUnlock.Unlocks;
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
        private MotionHandle _anchorMotionHandle;

        public Vector3 scaleStart = new(0.1f, 0.1f, 0.1f);
        public Vector3 scaleEnd = new(1, 1, 1);
        private MotionHandle _scaleMotionHandle;

        public TMP_Text upgradeTitle;
        public Color upgradeHighlightColor = new Color(0.01f, 0.63f, 0.02f, 1);
        public string upgradeTitleFormat = "Upgrade {0} To <color={1}>Level {2}</color>";
        public string unlockTitleFormat = "Unlock <color={0}>{1}</color>";
        public string maxLevelTitleFormat = "{0} at {1} level is already at max";

        public Color startColor = Color.green;
        public Color endColor = Color.white;
        private MotionHandle _nextLevelProgressBarMotionHandle;
        public ProgressBar upgradeLevelProgressBar;
        public ProgressBar upgradeNextLevelProgressBar;
        public Image upgradeNextLevelProgressBarImage;
        public TMP_Text upgradeTimeTitle;
        public TMPFormat upgradeTimeFormat;

        public TMP_Text startButtonTitle;
        public TMPFormat startCoinRequirementFormat;

        [SerializeField] private RectTransform upgradeSlotParent;

        public Button startButton;
        public Button closeButton;

        private PlayerFarmReference playerFarmReference;
        private EventShowItemRequired _eventShowItemRequired;
        private Action<bool> _onStartButtonPressed;
        private Action _onCancelButtonPressed;
        private ILevel _levelReference;
        private IRequirementForUpgradeScriptableReference _requirementForUpgradeReference;
        private IUpgrade _upgradeReference;
        private IUnlock _unlockReference;
        private IUpgradeUnlockPreview _upgradeUnlockPreview;

        private void Awake()
        {
            startColor = upgradeNextLevelProgressBarImage.color;
        }

        private int CoinRequirement(int level) =>
            _requirementForUpgradeReference.RequirementForUpgrades.GetRequirement(level).coin;

        public UnityTimeSpan UpgradeTimeRequirement(int level) =>
            _requirementForUpgradeReference.RequirementForUpgrades.GetRequirement(level).fullTime;

        public int WokerRequirement(int level) =>
            _requirementForUpgradeReference.RequirementForUpgrades.GetRequirement(level).worker.Value;

        public int GemRequirement(int level) =>
            _requirementForUpgradeReference.RequirementForUpgrades.GetRequirement(level).gem;


        private Pair<Item, int>[] ItemsRequirement(int level) =>
            _requirementForUpgradeReference.RequirementForUpgrades.GetRequirement(level).items;

        public void Show(RectTransform parentRect, Transform currentSelectedTransform,
            PlayerFarmReference playerFarm, EventShowItemRequired eventShowItemRequired,
            ITitle titleReference, ILevel levelReference,
            Action<bool> onStartButtonPressed, Action onCancelButtonPressed)
        {
            _onStartButtonPressed = onStartButtonPressed;
            _onCancelButtonPressed = onCancelButtonPressed;
            _levelReference = levelReference;
            this.playerFarmReference = playerFarm;
            _eventShowItemRequired = eventShowItemRequired;
            SetPanel(parentRect, currentSelectedTransform, titleReference, levelReference, onStartButtonPressed);
            upgradeTimeFormat.SetTimeFormat(UpgradeTimeRequirement(levelReference.Level));
            startCoinRequirementFormat.SetTextInt(CoinRequirement(levelReference.Level));
        }

        private void SetPanel(RectTransform parentRect, Transform currentSelectedTransform, ITitle titleReference,
            ILevel levelReference, Action<bool> onStartButtonPressed)
        {
            startButton.onClick.RemoveAllListeners();
            bool canSet = TrySetPanelInfo(parentRect, currentSelectedTransform, levelReference, onStartButtonPressed);
            if (!canSet) return;
            closeButton.onClick.AddListener(OnCancelButtonPressed);
            SetParent(parentRect);
            SetTitle(titleReference, levelReference);
            SetProgressBar(levelReference.Level);
        }

        private bool TrySetPanelInfo(RectTransform parentRect, Transform currentSelectedTransform,
            ILevel levelReference,
            Action<bool> onStartButtonPressed)
        {
            if (levelReference.Level.IsMax && MaxLevelPrompt(onStartButtonPressed)) return true;
            if (levelReference.Level.IsLocked && UnlockPrompt(currentSelectedTransform, onStartButtonPressed))
                return true;
            if (UpgradePrompt(currentSelectedTransform, levelReference.Level)) return true;
            return false;
        }

        private void SetParent(RectTransform parentRect, bool playAnimation = true)
        {
            selfRectTransform.SetParent(parentRect);

            if (_anchorMotionHandle.IsActive()) _anchorMotionHandle.Complete();
            _anchorMotionHandle = LMotion.Create(anchorStartPosition, anchorEndPosition, duration).WithEase(ease)
                .BindToAnchoredPosition3D(selfRectTransform);

            if (_scaleMotionHandle.IsActive()) _scaleMotionHandle.Complete();
            _scaleMotionHandle = LMotion.Create(scaleStart, scaleEnd, duration).WithEase(ease)
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

        private bool UpgradePrompt(Transform currentSelectedTransform, Level level)
        {
            _upgradeReference = currentSelectedTransform.GetComponent<IUpgrade>();
            if (_upgradeReference != null)
            {
                SetItemRequirement(currentSelectedTransform, level);
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

                return true;
            }

            Hide();
            return false;
        }

        private void OnDisable()
        {
            if (_anchorMotionHandle.IsActive()) _anchorMotionHandle.Complete();
            if (_scaleMotionHandle.IsActive()) _scaleMotionHandle.Complete();
            if (_nextLevelProgressBarMotionHandle.IsActive()) _nextLevelProgressBarMotionHandle.Complete();
        }

        private void SetItemRequirement(Transform currentSelectedTransform, Level level)
        {
            _requirementForUpgradeReference =
                currentSelectedTransform.GetComponent<IRequirementForUpgradeScriptableReference>();
            _eventShowItemRequired.Trigger(ItemsRequirement(level));
            _upgradeUnlockPreview = currentSelectedTransform.GetComponent<IUpgradeUnlockPreview>();
            if (_upgradeUnlockPreview != null) _upgradeUnlockPreview.ShowUpgradeUnlockPreview(upgradeSlotParent);
        }

        private void Upgrade()
        {
            var canUpgrade = _upgradeReference.CanUpgrade;
            if (canUpgrade) _upgradeReference.Upgrade();
            if (_upgradeUnlockPreview != null) _upgradeUnlockPreview.HideUpgradeUnlockPreview();
            _onStartButtonPressed?.Invoke(canUpgrade);
        }


        public void TimerSetup(float time)
        {
            // Implement the logic to setup the timer
        }

        private bool MaxLevelPrompt(Action<bool> onStartButtonPressed)
        {
            return true;
            // Implement the logic to show the max level prompt
        }

        private bool UnlockPrompt(Transform currentSelectedTransform, Action<bool> onStartButtonPressed)
        {
            _unlockReference = currentSelectedTransform.GetComponent<IUnlock>();
            if (_unlockReference != null)
            {
                SetItemRequirement(currentSelectedTransform, _levelReference.Level);
                if (_unlockReference.IsUnlocking)
                {
                    startButton.interactable = false;
                }
                else
                {
                    startButton.interactable = true;
                    startButtonTitle.text = "Unlock";
                    startButton.onClick.AddListener(Unlock);
                }

                return true;
            }

            return true;
        }

        private void Unlock()
        {
            var canUnlock = _unlockReference.CanUnlock;
            if (_upgradeUnlockPreview != null) _upgradeUnlockPreview.HideUpgradeUnlockPreview();
            if (canUnlock)
            {
                _unlockReference.Unlock();
            }

            _onStartButtonPressed?.Invoke(canUnlock);
        }

        private void SetProgressBar(Level level)
        {
            if (level.IsLocked)
            {
                upgradeLevelProgressBar.Value = 0.01f;
                upgradeNextLevelProgressBar.Value = 0.02f;
            }
            else
            {
                upgradeLevelProgressBar.SetValueWithoutNotify(level.Current / (float)level.Max);
                var maxIncrement = Mathf.Min(level.Max, level.Current + 1);
                upgradeNextLevelProgressBar.SetValueWithoutNotify(maxIncrement / (float)level.Max);
            }

            _nextLevelProgressBarMotionHandle = LMotion.Create(startColor, endColor, duration)
                .WithLoops(-1, LoopType.Yoyo)
                .WithEase(ease).BindToColor(upgradeNextLevelProgressBarImage);
        }


        private void OnCancelButtonPressed()
        {
            if (_upgradeUnlockPreview != null) _upgradeUnlockPreview.HideUpgradeUnlockPreview();
            _eventShowItemRequired.Trigger(null);
            _onCancelButtonPressed?.Invoke();
        }

        public void Hide()
        {
            if (_upgradeUnlockPreview != null) _upgradeUnlockPreview.HideUpgradeUnlockPreview();
            gameObject.SetActive(false);
            transform.SetParent(null);
        }
    }
}