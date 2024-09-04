using Pancake;
using Soul.Controller.Runtime.Items;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.UpgradeAndUnlock.Unlocks;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using Soul.Model.Runtime.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Controller.Runtime.UI.Manager
{
    public class SelectedTransformInfoComponent : GameComponent
    {
        private bool enableCall;
        // --- Events ---
        public Event<Transform> onUpgradeAbleOrUnlockAbleSelected;

        // --- UI Elements ---
        [Header("General")] public CanvasGroup canvasGroup;
        public SelectData selectData;

        [Header("Title Section")] public GameObject titleContainer;
        public TextMeshProUGUI titleText;

        [Header("Level Section")] public GameObject levelContainer;
        public TMPFormat levelTextFormat;

        [Header("Unlock/Upgrade Section")] public Button unlockUpgradeToggleButton;
        public Button unlockUpgradeActionButton;
        public TMP_Text unlockUpgradeActionTitle;
        public TMP_Text unlockUpgradeCoinRequirement;
        public TMP_Text unlockUpgradeTimeRequirement;
        public Image unlockUpgradeButtonImage;
        public TextMeshProUGUI unlockUpgradeButtonText;
        public TMPFormat unlockOrUpgradeTitleTextFormat;
        public GameObject unlockAndUpgradePanel;
        public Button closeUnlockUpgradePanelButton;
        
        //ScriptableObject
        public EventShowItemRequired eventShowItemRequired;

        // --- Sprites & Text ---
        [SerializeField] private Sprite unlockButtonSprite;
        [SerializeField] private Sprite upgradeButtonSprite;
        [SerializeField] private Sprite closeButtonSprite;
        [SerializeField] private string upgradeButtonText = "Upgrade";
        [SerializeField] private string unlockButtonText = "Unlock";
        [SerializeField] private string maxLevelText = "Max";
        [SerializeField] private string levelLockedText = "Lock";

        // --- Internal State ---
        private Transform currentSelectedTransform;

        private void OnEnable()
        {
            Hide();
        }

        private void Hide()
        {
            HideCanvas();
            unlockAndUpgradePanel.SetActive(false);
        }

        private void OnDisable()
        {
            RemoveAllButtonListeners();
        }


        public void OnSelected(Transform selectedTransform)
        {
            Hide();
            RemoveAllButtonListeners();
            currentSelectedTransform = selectedTransform;

            if (selectData.GetDataFrom(selectedTransform) > 0)
            {
                ShowCanvas();
                UpdateTitle(selectData.titleReference);
                UpdateUnlockUpgradeLevel(selectData.levelReference);
            }
            else
            {
                onUpgradeAbleOrUnlockAbleSelected.Trigger(null);
                HideCanvas();
            }
        }

        // --- Private Helper Methods ---

        private void RemoveAllButtonListeners()
        {
            unlockUpgradeToggleButton.onClick.RemoveAllListeners();
            unlockUpgradeActionButton.onClick.RemoveAllListeners();
            closeUnlockUpgradePanelButton.onClick.RemoveAllListeners();
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

        private void ShowUnlockUpgradePanel()
        {
            RemoveAllButtonListeners(); // Ensure only one listener is active
            closeUnlockUpgradePanelButton.onClick.AddListener(HideUnlockUpgradePanel);
            unlockUpgradeToggleButton.onClick.AddListener(HideUnlockUpgradePanel);
            unlockUpgradeButtonImage.sprite = closeButtonSprite;
            unlockAndUpgradePanel.SetActive(true);
            eventShowItemRequired.Trigger(selectData.ItemsRequirement(selectData.Level));
        }

        private void HideUnlockUpgradePanel()
        {
            if (currentSelectedTransform) OnSelected(currentSelectedTransform); // Refresh UI
            eventShowItemRequired.Trigger(null);
        }

        private void Unlock()
        {
            var unlockReference = currentSelectedTransform.GetComponent<IUnlock>();
            if (unlockReference.CanUnlock) unlockReference.Unlock();
            Debug.Log("Unlocking");
            OnSelected(currentSelectedTransform); // Refresh UI after unlock
        }

        private void Upgrade()
        {
            var upgradeReference = currentSelectedTransform.GetComponent<IUpgrade>();
            if (upgradeReference.CanUpgrade) upgradeReference.Upgrade();
            Debug.Log("Upgrading");
            OnSelected(currentSelectedTransform); // Refresh UI after upgrade
        }


        // --- UI Update Methods ---
        private void UpdateTitle(InterfaceFinder<ITitle> titleData)
        {
            titleContainer.SetActive(titleData);
            if (titleData) titleText.text = titleData.Value.Title;
        }

        private void UpdateUnlockUpgradeLevel(InterfaceFinder<ILevel> levelData)
        {
            if (levelData)
            {
                Level levelValue = levelData.Value.Level;
                unlockUpgradeToggleButton.gameObject.SetActive(true);
                levelContainer.SetActive(true);

                if (levelValue.IsLocked)
                {
                    ConfigureForLockedState();
                }
                else if (levelValue.IsMax)
                {
                    ConfigureForMaxLevelState(levelValue);
                }
                else
                {
                    ConfigureForUpgradableState(levelValue);
                }
            }
            else
            {
                onUpgradeAbleOrUnlockAbleSelected.Trigger(null);
                HideUnlockUpgradePanel();
                DisableUpgradeButton();
                DisableLevelContainer();
            }
        }

        private void ConfigureForLockedState()
        {
            unlockUpgradeActionButton.interactable = true;
            unlockUpgradeActionButton.onClick.AddListener(Unlock);
            onUpgradeAbleOrUnlockAbleSelected.Trigger(currentSelectedTransform);
            unlockUpgradeToggleButton.interactable = true;
            unlockUpgradeToggleButton.onClick.AddListener(ShowUnlockUpgradePanel);
            unlockUpgradeButtonImage.sprite = unlockButtonSprite;
            unlockUpgradeButtonText.text = unlockButtonText;
            unlockUpgradeActionTitle.text = "Unlock";
            levelTextFormat.TMP.text = levelLockedText;
            if (selectData.titleReference) unlockOrUpgradeTitleTextFormat.TMP.text = "Unlock " + selectData.titleReference;
            unlockUpgradeCoinRequirement.text = selectData.CurrencyRequirement(0).Value.ToString();
        }

        private void ConfigureForMaxLevelState(Level levelValue)
        {
            unlockUpgradeActionButton.interactable = false;
            onUpgradeAbleOrUnlockAbleSelected.Trigger(null);
            unlockUpgradeToggleButton.interactable = false;
            unlockUpgradeButtonImage.sprite = upgradeButtonSprite;
            unlockUpgradeButtonText.text = maxLevelText;
            levelTextFormat.SetTextInt(levelValue.Current);
            unlockUpgradeActionTitle.text = "Max";
            if (selectData.titleReference) unlockOrUpgradeTitleTextFormat.TMP.text = selectData.titleReference + " Maxed Level Reached";
        }

        private void ConfigureForUpgradableState(Level levelValue)
        {
            unlockUpgradeActionButton.interactable = true;
            unlockUpgradeActionButton.onClick.AddListener(Upgrade);
            onUpgradeAbleOrUnlockAbleSelected.Trigger(currentSelectedTransform);
            unlockUpgradeToggleButton.interactable = true;
            unlockUpgradeToggleButton.onClick.AddListener(ShowUnlockUpgradePanel);
            unlockUpgradeButtonImage.sprite = upgradeButtonSprite;
            unlockUpgradeButtonText.text = upgradeButtonText;
            unlockUpgradeActionTitle.text = "Upgrade";
            levelTextFormat.SetTextInt(levelValue.Current);
            unlockUpgradeCoinRequirement.text = selectData.CurrencyRequirement(levelValue).Value.ToString();
            // Simplified title formatting
            if (selectData.titleReference)
            {
                unlockOrUpgradeTitleTextFormat.TMP.text =
                    string.Format(selectData.titleReference.Value.Title, levelValue.Current);
            }
        }

        private void DisableLevelContainer()
        {
            levelContainer.SetActive(false);
        }

        private void DisableUpgradeButton()
        {
            unlockUpgradeToggleButton.gameObject.SetActive(false);
        }
    }
}