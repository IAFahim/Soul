using Pancake;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.UpgradeAndUnlock.Unlocks;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using Soul.Model.Runtime.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Soul.Presenter.Runtime.UI
{
    public class SelectedTransformInfoComponent : GameComponent
    {
        public Transform currentSelectedTransform;

        public CanvasGroup canvasGroup;
        public SelectData selectData;

        public GameObject titleContainer;
        public TextMeshProUGUI title;

        public TextMeshProUGUI unLockUpgradeButtonTMP;
        public Sprite unlockButtonSprite;
        public Sprite upgradeButtonSprite;
        public Sprite closeSprite;
        public Button closeButton;
        public string upgradeButtonText = "Upgrade";
        public string unlockButtonText = "Unlock";
        public string maxLevelText = "Max";

        [FormerlySerializedAs("unLockUpgradeButton")]
        public Button unLockUpgradeActivationButton;

        public Button unLockUpgradeStartButton;

        public Image unLockUpgradeImage;

        public TMPFormat unlockOrUpgradeTitleTMP;
        public GameObject unlockAndUpgradePanel;
        public VerticalLayoutGroup unlockOrUpgradeRequirementContainer;

        public GameObject levelContainer;
        public string levelLockedText = "Lock";
        public TMPFormat levelTMP;

        public Event<Transform> onUpgradeAbleOrUnlockAbleSelected;


        private void Awake()
        {
            levelTMP.StoreFormat();
            unlockOrUpgradeTitleTMP.StoreFormat();
        }

        private void OnEnable()
        {
            HideCanvas();
            HideUnlockPanel();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        public void RemoveListeners()
        {
            unLockUpgradeActivationButton.onClick.RemoveListener(ShowUnlockPanel);
            unLockUpgradeActivationButton.onClick.RemoveListener(HideUnlockPanel);
            unLockUpgradeStartButton.onClick.RemoveListener(Upgrade);
            unLockUpgradeStartButton.onClick.RemoveListener(Unlock);
            closeButton.onClick.RemoveListener(HideUnlockPanel);
        }

        public void OnSelected(Transform selectedTransform)
        {
            RemoveListeners();
            currentSelectedTransform = selectedTransform;
            var found = selectData.GetDataFrom(selectedTransform);
            if (found > 0)
            {
                ShowCanvas();
                TitleSetActive(selectData.title);
                UnlockUpgradeLevelSetActive(selectData.level);
            }
            else
            {
                onUpgradeAbleOrUnlockAbleSelected.Trigger(null);
                HideCanvas();
            }
        }

        private void TitleSetActive(InterfaceFinder<ITitle> selectDataTitle)
        {
            if (selectDataTitle) title.text = selectDataTitle.Value.Title;
            else titleContainer.SetActive(false);
        }

        private void UnlockUpgradeLevelSetActive(InterfaceFinder<ILevel> selectDataLevel)
        {
            if (selectDataLevel)
            {
                Level levelValue = selectDataLevel.Value.Level;
                unLockUpgradeActivationButton.gameObject.SetActive(true);
                levelContainer.SetActive(true);
                if (levelValue.IsLocked)
                {
                    MarkedLock();
                }
                else if (levelValue.IsMax)
                {
                    MarkMaxLevel(levelValue);
                }
                else
                {
                    MarkUpgrade(levelValue);
                }
            }
            else
            {
                onUpgradeAbleOrUnlockAbleSelected.Trigger(null);
                HideUnlockPanel();
                DisableUpgrade();
                DisableLevelContainer();
            }
        }

        public void ShowUnlockPanel()
        {
            RemoveListeners();
            closeButton.onClick.AddListener(HideUnlockPanel);
            unLockUpgradeActivationButton.onClick.AddListener(HideUnlockPanel);
            unLockUpgradeImage.sprite = closeSprite;
            unlockAndUpgradePanel.SetActive(true);
            unlockOrUpgradeRequirementContainer.gameObject.SetActive(true);
        }

        public void HideUnlockPanel()
        {
            closeButton.onClick.RemoveListener(HideUnlockPanel);
            unLockUpgradeActivationButton.onClick.RemoveListener(HideUnlockPanel);
            unlockAndUpgradePanel.SetActive(false);
            unlockOrUpgradeRequirementContainer.gameObject.SetActive(false);
            if (currentSelectedTransform) OnSelected(currentSelectedTransform);
        }

        private void Unlock()
        {
            var unlockReference = currentSelectedTransform.GetComponent<IUnlock>();
            if (unlockReference.CanUnlock) unlockReference.Unlock();
            OnSelected(currentSelectedTransform);
        }

        public void Upgrade()
        {
            var upgradeReference = currentSelectedTransform.GetComponent<IUpgrade>();
            if (upgradeReference.CanUpgrade) upgradeReference.Upgrade();
            OnSelected(currentSelectedTransform);
        }

        private void MarkUpgrade(Level levelValue)
        {
            unLockUpgradeStartButton.interactable = true;
            onUpgradeAbleOrUnlockAbleSelected.Trigger(currentSelectedTransform);
            unLockUpgradeActivationButton.interactable = true;
            unLockUpgradeActivationButton.onClick.AddListener(ShowUnlockPanel);
            unLockUpgradeImage.sprite = upgradeButtonSprite;
            unLockUpgradeButtonTMP.text = upgradeButtonText;
            levelTMP.SetTextInt(levelValue.Current);
            if (selectData.title)
                unlockOrUpgradeTitleTMP.TMP.text = string.Format(
                    unlockOrUpgradeTitleTMP, selectData.title, levelValue.Current
                );
        }

        private void MarkMaxLevel(Level levelValue)
        {
            unLockUpgradeStartButton.interactable = false;
            onUpgradeAbleOrUnlockAbleSelected.Trigger(null);
            unLockUpgradeActivationButton.interactable = false;
            unLockUpgradeImage.sprite = upgradeButtonSprite;
            unLockUpgradeButtonTMP.text = maxLevelText;
            levelTMP.SetTextInt(levelValue.Current);
            if (selectData.title) unlockOrUpgradeTitleTMP.TMP.text = selectData.title + " Maxed Level Reached";
        }

        private void MarkedLock()
        {
            unLockUpgradeStartButton.interactable = true;
            unLockUpgradeStartButton.onClick.AddListener(Unlock);
            onUpgradeAbleOrUnlockAbleSelected.Trigger(currentSelectedTransform);
            unLockUpgradeActivationButton.interactable = true;
            unLockUpgradeActivationButton.onClick.AddListener(ShowUnlockPanel);
            unLockUpgradeImage.sprite = unlockButtonSprite;
            unLockUpgradeButtonTMP.text = unlockButtonText;
            levelTMP.TMP.text = levelLockedText;
            if (selectData.title) unlockOrUpgradeTitleTMP.TMP.text = "Unlock " + selectData.title;
        }

        private void DisableLevelContainer()
        {
            levelContainer.SetActive(false);
        }

        public void DisableUpgrade()
        {
            unLockUpgradeActivationButton.gameObject.SetActive(false);
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
    }
}