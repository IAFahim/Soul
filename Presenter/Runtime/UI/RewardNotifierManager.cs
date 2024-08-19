using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Pools;
using QuickEye.Utility;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Presenter.Runtime.UI
{
    public class RewardNotifierManager : GameComponent, IRemoveReference<Item>
    {
        public PlayerInventoryReference playerInventoryReference;
        public Transform spawnPoint;
        public RewardNotifier prefab;
        public UnityDictionary<Item, RewardNotifier> instantiatedRewardNotifiers;

        private void OnEnable()
        {
            playerInventoryReference.inventory.OnAddedOrIncreased += OnAddedOrIncreased;
        }

        private void OnAddedOrIncreased(Item item, int amount, int _, bool __)
        {
            OnAddedOrIncreasedAsync(item, amount, _, __).Forget();
        }


        private async UniTask OnAddedOrIncreasedAsync(Item item, int amount, int _, bool __)
        {
            if (!instantiatedRewardNotifiers.TryGetValue(item, out var rewardNotifier))
            {
                rewardNotifier = prefab.gameObject.Request<RewardNotifier>(spawnPoint);
                instantiatedRewardNotifiers.Add(item, rewardNotifier);
            }

            await UniTask.WaitForEndOfFrame(this);
            rewardNotifier.Setup(amount, playerInventoryReference.weight, item, this);
        }
        
        public void RemoveSelf(Item self)
        {
            instantiatedRewardNotifiers.Remove(self);
        }

        private void OnDisable()
        {
            playerInventoryReference.inventory.OnAddedOrIncreased -= OnAddedOrIncreased;
        }
    }
}