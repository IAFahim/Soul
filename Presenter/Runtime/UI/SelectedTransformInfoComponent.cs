using Pancake;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Unlocks;
using Soul.Model.Runtime.Upgrades;
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

        [FormerlySerializedAs("upgradeButton")]
        public Button unLockUpgradeButton;

        public Sprite unlockButtonSprite;
        public Sprite upgradeButtonSprite;
        public string upgradeButtonText = "Upgrade";
        public string unlockButtonText = "Unlock";
        public string maxLevelText = "Max";
        public Image unLockUpgradeImage;
        public TextMeshProUGUI unLockUpgradeButtonTMP;

        public GameObject levelContainer;
        public string levelLockedText = "Lock";
        public TMPFormat levelTMP;


        private void Awake()
        {
            levelTMP.StoreFormat();
        }

        private void OnEnable()
        {
            HideCanvas();
        }

        private void OnDisable()
        {
            RemoveAllListeners();
        }

        public void RemoveAllListeners()
        {
            unLockUpgradeButton.onClick.RemoveListener(Unlock);
            unLockUpgradeButton.onClick.RemoveListener(Upgrade);
        }

        public void OnSelected(Transform selectedTransform)
        {
            RemoveAllListeners();
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
                unLockUpgradeButton.gameObject.SetActive(true);
                levelContainer.SetActive(true);
                if (levelValue.IsLocked)
                {
                    MarkedLock();
                }
                else if (levelValue.IsMaxLevel)
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
                DisableUpgrade();
                DisableLevelContainer();
            }
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
            unLockUpgradeButton.interactable = true;
            unLockUpgradeButton.onClick.AddListener(Upgrade);
            unLockUpgradeImage.sprite = upgradeButtonSprite;
            unLockUpgradeButtonTMP.text = upgradeButtonText;
            levelTMP.SetTextInt(levelValue.CurrentLevel);
        }

        private void MarkMaxLevel(Level levelValue)
        {
            unLockUpgradeButton.interactable = false;
            unLockUpgradeImage.sprite = upgradeButtonSprite;
            unLockUpgradeButtonTMP.text = maxLevelText;
            levelTMP.SetTextInt(levelValue.CurrentLevel);
        }
        
        private void MarkedLock()
        {
            unLockUpgradeButton.interactable = true;
            unLockUpgradeButton.onClick.AddListener(Unlock);
            unLockUpgradeImage.sprite = unlockButtonSprite;
            unLockUpgradeButtonTMP.text = unlockButtonText; 
            levelTMP.TMP.text = levelLockedText;
        }

        private void DisableLevelContainer()
        {
            levelContainer.SetActive(false);
        }

        public void DisableUpgrade()
        {
            unLockUpgradeButton.gameObject.SetActive(false);
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