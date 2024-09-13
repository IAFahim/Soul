using Pancake;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.UIs;
using Soul.Model.Runtime.Variables;
using Soul.Presenter.Runtime.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Manager
{
    public class SelectedTransformInfoComponent : GameComponent, IHideCallBack
    {
        private bool enableCall;
        public PlayerInventoryReference playerInventoryReference;
        public EventShowItemRequired eventShowItemRequired;

        // --- Events ---
        public Event<(Transform transform, IHideCallBack hideCallBack)> onUpgradeAbleOrUnlockAbleSelected;

        // --- UI Elements ---
        [Header("General")] public CanvasGroup canvasGroup;
        public SelectData selectData;

        [Header("Title Section")] public GameObject titleContainer;
        public TextMeshProUGUI titleText;

        [Header("Level Section")] public GameObject levelContainer;
        public TMPFormat levelTextFormat;

        [Header("Upgrade Unlock Section")] [SerializeField]
        Pair<string, Sprite> maxButtonInfo = new("Max", null);
        [SerializeField] Pair<string, Sprite> unlockButtonInfo = new("Unlock", null);
        [SerializeField] Pair<string, Sprite> upgradeButtonInfo = new("Upgrade", null);

        [SerializeField] private Image unlockUpgradeToggleButtonImage;
        [SerializeField] private TMP_Text unlockUpgradeButtonText;
        [SerializeField] private Button unlockUpgradeActionButton;

        [Header("Upgrade Unlock Panel")] [SerializeField]
        private RectTransform upgradeUnlockPanelParent;

        [SerializeField] private UpgradeUnlockPanel upgradeUnlockPanel;

        // --- Internal State ---
        private Transform _currentSelectedTransform;

        public SelectedTransformInfoComponent(bool enableCall)
        {
            this.enableCall = enableCall;
        }

        private void OnEnable() => Hide();

        private void Hide()
        {
            upgradeUnlockPanel.Hide();
            HideCanvas();
        }

        public void OnSelected(Transform selectedTransform)
        {
            Hide();
            _currentSelectedTransform = selectedTransform;

            if (selectData.GetDataFrom(selectedTransform) > 0)
            {
                ShowCanvas();
                SetClip(selectData.titleReference, selectData.levelReference);
                UpdateUnlockButton(selectData.levelReference);
            }
            else
            {
                HideCanvas();
            }
        }

        private void SetClip(ComponentFinder<ITitle> selectDataTitleReference, ComponentFinder<ILevel> selectDataLevelReference)
        {
            UpdateTitle(selectDataTitleReference);
            ShowLevel(selectDataLevelReference);
        }
        
        private void UpdateTitle(ComponentFinder<ITitle> titleData)
        {
            titleContainer.SetActive(titleData);
            if (titleData) titleText.text = titleData.Value.Title;
        }

        private void ShowLevel(ComponentFinder<ILevel> levelData)
        {
            if (!levelData) return;
            Level levelValue = levelData.Value.Level;
            if (levelValue.IsMax) levelTextFormat.TMP.text = maxButtonInfo.Key; 
            else if (levelValue.IsLocked) levelTextFormat.TMP.text = unlockButtonInfo.Key;
            else levelTextFormat.SetTextInt(levelValue.Current);
        }

        private void UpdateUnlockButton(ComponentFinder<ILevel> selectDataLevelReference)
        {
            unlockUpgradeActionButton.onClick.RemoveAllListeners();
            if (selectDataLevelReference)
            {
                unlockUpgradeActionButton.gameObject.SetActive(true);
                unlockUpgradeActionButton.onClick.AddListener(ShowUpdateUnlockPanel);
                var level = selectDataLevelReference.Value.Level;
                if (level.IsMax)
                {
                    unlockUpgradeButtonText.text = maxButtonInfo.Key;
                    unlockUpgradeToggleButtonImage.sprite = maxButtonInfo.Value;
                    unlockUpgradeActionButton.interactable = false;
                }
                else if (level.IsLocked)
                {
                    unlockUpgradeButtonText.text = unlockButtonInfo.Key;
                    unlockUpgradeToggleButtonImage.sprite = unlockButtonInfo.Value;
                    unlockUpgradeActionButton.interactable = true;
                }
                else
                {
                    unlockUpgradeButtonText.text = upgradeButtonInfo.Key;
                    unlockUpgradeToggleButtonImage.sprite = upgradeButtonInfo.Value;
                    unlockUpgradeActionButton.interactable = true;
                }
            }
            else
            {
                unlockUpgradeActionButton.gameObject.SetActive(false);
                upgradeUnlockPanel.Hide();
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

       

        private void ShowUpdateUnlockPanel()
        {
            if (selectData.levelReference)
            {
                upgradeUnlockPanel.Show(
                    upgradeUnlockPanelParent, _currentSelectedTransform,
                    playerInventoryReference, eventShowItemRequired,
                    selectData.titleReference.Value, selectData.levelReference.Value,
                    OnUpgradeUnlockStartButtonPressed
                );
            }
        }

        private void OnUpgradeUnlockStartButtonPressed()
        {
        }

        private void DisableLevelContainer()
        {
            levelContainer.SetActive(false);
        }

        public void HideCallBack()
        {
            Hide();
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

        //


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
    }
}