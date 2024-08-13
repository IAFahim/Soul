using Pancake;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.UI
{
    public class SelectedDataComponent : GameComponent
    {
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
        public TextMeshProUGUIFormat levelTMP;
        
        


        private void Awake()
        {
            levelTMP.StoreFormat();
        }

        private void OnEnable()
        {
            HideCanvas();
        }

        public void OnSelected(Transform selectedTransform)
        {
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
                    unLockUpgradeButton.interactable = true;
                    unLockUpgradeImage.sprite = unlockButtonSprite;
                    unLockUpgradeButtonTMP.text = unlockButtonText;
                    levelTMP.TMP.text = levelLockedText;
                }
                else if (levelValue.AtMaxLevel)
                {
                    unLockUpgradeButton.interactable = false;
                    unLockUpgradeImage.sprite = upgradeButtonSprite;
                    unLockUpgradeButtonTMP.text = maxLevelText;
                    levelTMP.SetTextInt(levelValue.CurrentLevel);
                }
                else
                {
                    unLockUpgradeButton.interactable = true;
                    unLockUpgradeImage.sprite = upgradeButtonSprite;
                    unLockUpgradeButtonTMP.text = upgradeButtonText;
                    levelTMP.SetTextInt(levelValue.CurrentLevel);
                }
            }
            else
            {
                DisableUpgrade();
                DisableLevelContainer();
            }
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