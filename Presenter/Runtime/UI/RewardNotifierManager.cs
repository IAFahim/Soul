using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Pools;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Inventories;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Limits;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Presenter.Runtime.UI
{
    public class RewardNotifierManager : GameComponent, IRemoveCallBack<Item>
    {
        [FormerlySerializedAs("playerInventoryReference")] public PlayerFarmReference playerFarmReference;
        public Transform spawnPoint;
        public RewardNotifier prefab;
        private Dictionary<Item, PairClass<RewardNotifier, int>> _instantiatedRewardNotifiers;

        private void OnEnable()
        {
            _instantiatedRewardNotifiers = new Dictionary<Item, PairClass<RewardNotifier, int>>();
            playerFarmReference.inventory.OnItemChanged += OnAddedOrIncreased;
        }

        private void OnAddedOrIncreased(InventoryChangeEventArgs<Item, int> inventoryChangeEventArgs)
        {
            OnAdded(inventoryChangeEventArgs.Key,inventoryChangeEventArgs.NewAmount,
                inventoryChangeEventArgs.ChangeAmount, inventoryChangeEventArgs.ChangeType);
        }

        private void OnAdded(Item key, int newAmount, int changeAmount, InventoryChangeType changeType)
        {
            if (changeType == InventoryChangeType.Added || changeType == InventoryChangeType.Increased)
            {
                OnAddedOrIncreasedAsync(key, newAmount, changeAmount, changeType).Forget();
            }
        }


        private async UniTask OnAddedOrIncreasedAsync(Item item, int newAmount, int changeAmount,
            InventoryChangeType changeType)
        {
            if (!_instantiatedRewardNotifiers.TryGetValue(item, out var rewardNotifier))
            {
                rewardNotifier =
                    new PairClass<RewardNotifier, int>(prefab.gameObject.Request<RewardNotifier>(spawnPoint), 0);
                _instantiatedRewardNotifiers.Add(item, rewardNotifier);
            }

            rewardNotifier.Value++;
            await UniTask.WaitForEndOfFrame(this);
            Limit weightLimitInt = playerFarmReference.weight;
            rewardNotifier.Key.Setup(item, newAmount, changeAmount, weightLimitInt, this);
        }

        public void RemoveSelf(Item self)
        {
            var keyValuePair = _instantiatedRewardNotifiers[self];
            keyValuePair.Value--;
            if (keyValuePair.Value == 0)
            {
                keyValuePair.Key.ReturnToPool();
                _instantiatedRewardNotifiers.Remove(self);
            }
        }

        private void OnDisable()
        {
            playerFarmReference.inventory.OnItemChanged -= OnAddedOrIncreased;
            _instantiatedRewardNotifiers.Clear();
        }
    }
}