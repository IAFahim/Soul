using Pancake;
using QuickEye.Utility;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings.Managers
{
    public class CropProductionManager : GameComponent, ISingleDrop, IWeightCapacity
    {
        public PlayerInventoryReference playerInventoryReference;

        [SerializeField]
        private RequirementOfWorkerGroupTimeCurrencyForLevels requirementOfWorkerGroupTimeCurrencyForLevels;

        public int capacity;
        public float totalWorkerCount = 5;

        public Pair<Item, int> productionItem;
        public Pair<Item, int> coinRequirement;
        public UnityTimeSpan productionTime;
        public int storedWorkers;

        public ProductionRequirement requirement; 

        public Item queueItem;
        public int maxAllowedWorkersRequirement;

        public float WeightLimit => capacity;


        public void TempAdd(Item[] items)
        {
            queueItem = items[0];
            playerInventoryReference.inventoryPreview.AddOrIncrease(queueItem, (int)WeightLimit);
            playerInventoryReference.inventoryPreview.AddOrIncrease(coinRequirement.Key, coinRequirement.Value);
            productionItem = new Pair<Item, int>(queueItem, (int)WeightLimit);
        }

        public void StartProduction()
        {
            if (HasEnoughToStart()) OnReward(false);
        }

        private void OnReward(bool isEnd)
        {
            playerInventoryReference.inventory.Decrease(queueItem, (int)WeightLimit);
            playerInventoryReference.inventory.Decrease(coinRequirement.Key, coinRequirement.Value);
            if (!isEnd) storedWorkers = maxAllowedWorkersRequirement;
        }

        public bool HasEnoughToStart()
        {
            return playerInventoryReference.inventory.HasEnough(coinRequirement) &&
                   playerInventoryReference.inventory.HasEnough(productionItem) &&
                   totalWorkerCount >= maxAllowedWorkersRequirement;
        }

        public bool Setup(ProductionRequirement productionRequirement)
        {
            requirement = productionRequirement;
            return true;
        }
    }
}