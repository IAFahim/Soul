using Pancake;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Workers;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings.Managers
{
    public class CropProductionManager : GameComponent, ISaveAble, ISingleDrop, IWeightCapacity
    {
        public const string KeyPrefix = "prod";
        public ItemInventoryReference inventoryReference;
        [SerializeField] private RequirementOfWorkerGroupTimeCurrencyForLevels requirementOfWorkerGroupTimeCurrencyForLevels; 
        public int capacity;
        public float totalWorkerCount = 5;

        public Pair<Item, int> productionItem;
        public Pair<Item, int> coinRequirement;
        public WorkerGroup storedWorkers;

        public Item queueItem;
        public WorkerGroup maxAllowedWorkersRequirement;

        public float WeightLimit => capacity;
        

        public void TempAdd(Item[] items)
        {
            queueItem = items[0];
            inventoryReference.tempInventory.AddOrIncreaseItem(queueItem, (int)WeightLimit);
            inventoryReference.tempInventory.AddOrIncreaseItem(coinRequirement.Key, coinRequirement.Value);
            productionItem = new Pair<Item, int>(queueItem, (int)WeightLimit);
        }

        public void StartProduction()
        {
            if (HasEnoughToStart()) OnReward(false);
        }

        private void OnReward(bool isEnd)
        {
            inventoryReference.inventory.DecreaseItem(queueItem, (int)WeightLimit);
            inventoryReference.inventory.DecreaseItem(coinRequirement.Key, coinRequirement.Value);
            if (!isEnd) storedWorkers.Subtract(maxAllowedWorkersRequirement.count);
        }

        public bool HasEnoughToStart()
        {
            return inventoryReference.inventory.HasEnoughItem(coinRequirement) &&
                   inventoryReference.inventory.HasEnoughItem(productionItem) &&
                   totalWorkerCount >= maxAllowedWorkersRequirement.count;
        }

        public void Save(string key)
        {
            // Data.Save(key + KeyPrefix, ProductionItem);
        }
    }
}