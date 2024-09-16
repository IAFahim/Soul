using Cysharp.Threading.Tasks;
using Pancake.Monetization;
using Pancake.UI;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.Bags;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.Views
{
    public class AdsRewardView : View
    {
        [SerializeField] PlayerFarmReference playerFarmReference;
        [SerializeField] private Button closeButton;
        [SerializeField] private AdsRewardBag adsRewardBag;
        [SerializeField] private Button watchAdsButton;
        [SerializeField] private GridLayoutGroup gridLayout;

        [FormerlySerializedAs("itemCount")] [SerializeField]
        private GameObject itemCountPrefab;

        protected override UniTask Initialize()
        {
            Advertising.Reward.Load();
            closeButton.onClick.AddListener(OnCloseButtonPressed);
            watchAdsButton.onClick.AddListener(OnWatchAdsButtonPressed);
            SpawnItems();
            return UniTask.CompletedTask;
        }

        private void SpawnItems()
        {
            foreach (var variable in adsRewardBag.rewardDictionary)
            {
                var itemCount = Instantiate(itemCountPrefab, gridLayout.transform);
                itemCount.GetComponentInChildren<Image>().sprite = variable.Key.icon;
                itemCount.GetComponentInChildren<TMP_Text>().text = $"x{variable.Value}";
            }
        }

        private void OnWatchAdsButtonPressed()
        {
            Advertising.Reward.Show()
                // .OnDisplayed(() => { Log("[ADVERTISING]: rewarded displayed"); })
                .OnClosed(OnCloseButtonPressed)
                // .OnLoaded(() => { Log("[ADVERTISING]: rewarded loaded"); })
                // .OnFailedToLoad(() => { Log("[ADVERTISING]: rewarded failed to load"); })
                // .OnFailedToDisplay(() => { Log("[ADVERTISING]: rewarded failed to display"); })
                .OnCompleted(CollectReward);
            // .OnSkipped(() => { Log("[ADVERTISING]: rewarded skipped"); });
        }

        private void CollectReward()
        {
            foreach (var variable in adsRewardBag.rewardDictionary)
            {
                playerFarmReference.inventory.AddOrIncrease(variable.Key, variable.Value, false);
            }

            playerFarmReference.inventory.Save();
            OnCloseButtonPressed();
        }


        private void OnCloseButtonPressed()
        {
            PopupHelper.Close(Transform);
        }
    }
}