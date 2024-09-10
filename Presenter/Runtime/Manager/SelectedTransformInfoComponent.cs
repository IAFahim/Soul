using Pancake;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.UIs;
using Soul.Model.Runtime.UpgradeAndUnlock.Unlocks;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using Soul.Model.Runtime.Variables;
using Soul.Presenter.Runtime.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Manager
{
    public class SelectedTransformInfoComponent : GameComponent, IHideCallBack
    {
        private bool enableCall;

        // --- Events ---
        public Event<(Transform transform, IHideCallBack hideCallBack)> onUpgradeAbleOrUnlockAbleSelected;

        // --- UI Elements ---
        [Header("General")] public CanvasGroup canvasGroup;
        public SelectData selectData;

        [Header("Title Section")] public GameObject titleContainer;
        public TextMeshProUGUI titleText;

        [Header("Level Section")] public GameObject levelContainer;
        public TMPFormat levelTextFormat;

        [SerializeField] private UpgradeUnlockPanel upgradeUnlockPanel;
        // --- Internal State ---
        private Transform currentSelectedTransform;

        private void OnEnable()
        {
            Hide();
        }

        private void Hide()
        {
            HideCanvas();
        }
        


        public void OnSelected(Transform selectedTransform)
        {
            Hide();
            currentSelectedTransform = selectedTransform;

            if (selectData.GetDataFrom(selectedTransform) > 0)
            {
                ShowCanvas();
                UpdateTitle(selectData.titleReference);
                UpdateUnlockUpgradeLevel(selectData.levelReference);
            }
            else
            {
                HideCanvas();
            }
        }

        

        private void ShowCanvas()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private void HideCanvas()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        // private void ShowUnlockUpgradePanel()
        // {
        //     closeUnlockUpgradePanelButton.onClick.RemoveAllListeners();
        //     closeUnlockUpgradePanelButton.onClick.AddListener(HideUnlockUpgradePanel);
        //     unlockUpgradeToggleButton.onClick.AddListener(HideUnlockUpgradePanel);
        //     unlockUpgradeToggleButtonImage.sprite = closeButtonSprite;
        //     unlockAndUpgradePanel.SetActive(true);
        //     eventShowItemRequired.Trigger(selectData.ItemsRequirement(selectData.Level));
        // }
        //
        // private void HideUnlockUpgradePanel()
        // {
        //     unlockAndUpgradePanel.SetActive(false);
        //     RemoveButtonListeners();
        //     unlockUpgradeToggleButton.onClick.AddListener(ShowUnlockUpgradePanel);
        //     unlockUpgradeToggleButtonImage.sprite = _lastToggleSprite;
        //     eventShowItemRequired.Trigger(null);
        // }
        //
        // private void Unlock()
        // {
        //     var unlockReference = currentSelectedTransform.GetComponent<IUnlock>();
        //     if (unlockReference.CanUnlock)
        //     {
        //         unlockReference.Unlock();
        //         eventShowItemRequired.Trigger(null);
        //     }
        //     OnSelected(currentSelectedTransform); // Refresh UI after unlock
        // }

        // private void Upgrade()
        // {
        //     var upgradeReference = currentSelectedTransform.GetComponent<IUpgrade>();
        //     if (upgradeReference.CanUpgrade)
        //     {
        //         upgradeReference.Upgrade();
        //         eventShowItemRequired.Trigger(null);
        //     }
        //     OnSelected(currentSelectedTransform); // Refresh UI after upgrade
        // }
        //
        //
        // // --- UI Update Methods ---
        private void UpdateTitle(InterfaceFinder<ITitle> titleData)
        {
            titleContainer.SetActive(titleData);
            if (titleData) titleText.text = titleData.Value.Title;
        }
        //
        private void UpdateUnlockUpgradeLevel(InterfaceFinder<ILevel> levelData)
        {
            if (levelData)
            {
                Level levelValue = levelData.Value.Level;
            }
            else
            {
                onUpgradeAbleOrUnlockAbleSelected.Trigger((null, this));
                DisableLevelContainer();
            }
        }

        // private void ConfigureForLockedState()
        // {
        //     unlockUpgradeActionButton.interactable = true;
        //     unlockUpgradeActionButton.onClick.AddListener(Unlock);
        //     onUpgradeAbleOrUnlockAbleSelected.Trigger((currentSelectedTransform, this));
        //     unlockUpgradeToggleButton.interactable = true;
        //     unlockUpgradeToggleButton.onClick.AddListener(ShowUnlockUpgradePanel);
        //     unlockUpgradeToggleButtonImage.sprite = _lastToggleSprite = unlockButtonSprite;
        //     unlockUpgradeButtonText.text = unlockButtonText;
        //     unlockUpgradeActionTitle.text = "Unlock";
        //     levelTextFormat.TMP.text = levelLockedText;
        //     if (selectData.titleReference)
        //         unlockOrUpgradeTitleTextFormat.TMP.text = "Unlock " + selectData.titleReference;
        //     unlockUpgradeCoinRequirement.text = selectData.CurrencyRequirement(0).Value.ToString();
        // }
        //
        // private void ConfigureForMaxLevelState(Level levelValue)
        // {
        //     unlockUpgradeActionButton.interactable = false;
        //     onUpgradeAbleOrUnlockAbleSelected.Trigger((null, this));
        //     unlockUpgradeToggleButton.interactable = false;
        //     unlockUpgradeToggleButtonImage.sprite = _lastToggleSprite = upgradeButtonSprite;
        //     unlockUpgradeButtonText.text = maxLevelText;
        //     levelTextFormat.SetTextInt(levelValue.Current);
        //     unlockUpgradeActionTitle.text = "Max";
        //     if (selectData.titleReference)
        //         unlockOrUpgradeTitleTextFormat.TMP.text = selectData.titleReference + " Maxed Level Reached";
        // }
        //
        // private void ConfigureForUpgradableState(Level levelValue)
        // {
        //     unlockUpgradeActionButton.interactable = true;
        //     unlockUpgradeActionButton.onClick.AddListener(Upgrade);
        //     onUpgradeAbleOrUnlockAbleSelected.Trigger((currentSelectedTransform, this));
        //     unlockUpgradeToggleButton.interactable = true;
        //     unlockUpgradeToggleButton.onClick.AddListener(ShowUnlockUpgradePanel);
        //     unlockUpgradeToggleButtonImage.sprite = _lastToggleSprite = upgradeButtonSprite;
        //     unlockUpgradeButtonText.text = upgradeButtonText;
        //     unlockUpgradeActionTitle.text = "Upgrade";
        //     levelTextFormat.SetTextInt(levelValue.Current);
        //     unlockUpgradeCoinRequirement.text = selectData.CurrencyRequirement(levelValue).Value.ToString();
        //     // Simplified title formatting
        //     if (selectData.titleReference)
        //     {
        //         unlockOrUpgradeTitleTextFormat.TMP.text =
        //             string.Format(selectData.titleReference.Value.Title, levelValue.Current);
        //     }
        // }

        private void DisableLevelContainer()
        {
            levelContainer.SetActive(false);
        }

        public void HideCallBack()
        {
            Hide();
        }
    }
}