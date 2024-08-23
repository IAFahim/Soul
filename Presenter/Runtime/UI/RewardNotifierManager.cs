using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Pools;
using QuickEye.Utility;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.Inventories;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Limits;
using UnityEngine;

namespace Soul.Presenter.Runtime.UI
{
    public class RewardNotifierManager : GameComponent, IRemoveSelfCallBack<Item>
    {
        public PlayerInventoryReference playerInventoryReference;
        public Transform spawnPoint;
        public RewardNotifier prefab;
        public UnityDictionary<Item, RewardNotifier> instantiatedRewardNotifiers;

        private void OnEnable()
        {
            playerInventoryReference.inventory.OnItemChanged += OnAddedOrIncreased;
        }

        private void OnAddedOrIncreased(InventoryChangeEventArgs<Item, int> inventoryChangeEventArgs)
        {
            OnAdded(inventoryChangeEventArgs.Key, inventoryChangeEventArgs.NewAmount,
                inventoryChangeEventArgs.ChangeAmount, inventoryChangeEventArgs.ChangeType);
        }

        private void OnAdded(Item key, int newAmount, int changeAmount, InventoryChangeType changeType)
        {
            if (changeType == InventoryChangeType.Added || changeType == InventoryChangeType.Increased)
            {
                OnAddedOrIncreasedAsync(key, newAmount, changeAmount, changeType).Forget();
            }
        }


        private async UniTask OnAddedOrIncreasedAsync(Item item, int newAmount, int changeAmount, InventoryChangeType changeType)
        {
            if (!instantiatedRewardNotifiers.TryGetValue(item, out var rewardNotifier))
            {
                rewardNotifier = prefab.gameObject.Request<RewardNotifier>(spawnPoint);
                instantiatedRewardNotifiers.Add(item, rewardNotifier);
            }

            await UniTask.WaitForEndOfFrame(this);
            LimitIntStruct weightLimitInt = playerInventoryReference.weight;
            rewardNotifier.Setup(item, newAmount, changeAmount, weightLimitInt, this);
        }

        public void RemoveSelf(Item self)
        {
            instantiatedRewardNotifiers.Remove(self);
        }

        private void OnDisable()
        {
            playerInventoryReference.inventory.OnItemChanged -= OnAddedOrIncreased;
        }
    }
}