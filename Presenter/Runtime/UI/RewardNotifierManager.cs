using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Pools;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.Inventories;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Limits;
using UnityEngine;

namespace Soul.Presenter.Runtime.UI
{
    public class RewardNotifierManager : GameComponent, IRemoveCallBack<Item>
    {
        public PlayerInventoryReference playerInventoryReference;
        public Transform spawnPoint;
        public RewardNotifier prefab;
        private Dictionary<Item, RewardNotifier> _instantiatedRewardNotifiers;

        private void OnEnable()
        {
            _instantiatedRewardNotifiers = new Dictionary<Item, RewardNotifier>();
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
            if (!_instantiatedRewardNotifiers.TryGetValue(item, out var rewardNotifier))
            {
                rewardNotifier = prefab.gameObject.Request<RewardNotifier>(spawnPoint);
                _instantiatedRewardNotifiers.Add(item, rewardNotifier);
            }

            await UniTask.WaitForEndOfFrame(this);
            LimitIntStruct weightLimitInt = playerInventoryReference.weight;
            rewardNotifier.Setup(item, newAmount, changeAmount, weightLimitInt, this);
        }

        public void RemoveSelf(Item self)
        {
            _instantiatedRewardNotifiers.Remove(self);
        }

        private void OnDisable()
        {
            playerInventoryReference.inventory.OnItemChanged -= OnAddedOrIncreased;
            _instantiatedRewardNotifiers.Clear();
        }
    }
}