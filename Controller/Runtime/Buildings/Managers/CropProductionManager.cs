using System;
using Pancake;
using QuickEye.Utility;
using Soul.Controller.Runtime.Converters;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Peoples.Workers;
using Soul.Model.Runtime.Requirements;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings.Managers
{
    public class CropProductionManager : GameComponent, ISingleDrop, IWeightCapacity
    {
        public PlayerInventoryReference playerInventoryReference;
        public ProductionRequirement currentProductionRequirement;

        [SerializeField]
        private RequirementOfWorkerGroupTimeCurrencyForLevels requirementOfWorkerGroupTimeCurrencyForLevels;

        public int capacity;
        public int currentLevel;
        public WorkerType basicWorkerType;

        public ItemToItemConverter itemToItemConverter;

        public Item queueItem;

        private Item productionItem;

        public Item ProductionItem
        {
            get
            {
                if (productionItem == null) return queueItem;
                return null;
            }
        }

        public float WeightLimit => capacity;
        public bool IsProducing => currentProductionRequirement.isProducing;
        public int CurrencyRequirement => 0;
        public int WorkerCount => currentProductionRequirement.workerCount.Value;
        public UnityDateTime StartTime => currentProductionRequirement.startTime;
        public UnityTimeSpan ReductionTime => currentProductionRequirement.reductionTime;
        public UnityTimeSpan RequiredTimeSpan => Required.time - ReductionTime;
        public UnityDateTime EndTime => StartTime + RequiredTimeSpan;
        public UnityTimeSpan TimeLeft => EndTime - DateTime.UtcNow;
        public bool IsCompleted => IsProducing && TimeLeft <= TimeSpan.Zero;

        public WorkerGroupTimeCurrencyRequirement<Item, int> Required =>
            requirementOfWorkerGroupTimeCurrencyForLevels.GetRequirement(currentLevel);

        public TimeSpan TimeRequired => Required.time;
        public int CurrentCurrency => playerInventoryReference.coins.Value;


        public void TempAdd(Item[] items)
        {
            queueItem = items[0];
            playerInventoryReference.inventoryPreview.AddOrIncrease(queueItem, (int)WeightLimit);
            playerInventoryReference.workerInventoryPreview.Decrease(basicWorkerType, Required.workerCount);
        }

        public void StartProduction()
        {
            if (HasEnoughToStart())
            {
                OnReward(false);
            }
        }

        private void OnReward(bool isEnd)
        {
            playerInventoryReference.inventory.Decrease(queueItem, (int)WeightLimit);
            if (itemToItemConverter.TryConvert(ProductionItem, out var convertedItem))
            {
                int productionAmount = (int)(convertedItem.Value * capacity);
                playerInventoryReference.inventory.AddOrIncrease(convertedItem.Key, productionAmount);
            }

            playerInventoryReference.coins.Set(CurrentCurrency + 10);
        }

        public bool HasEnoughToStart()
        {
            return playerInventoryReference.inventory.HasEnough(ProductionItem, capacity);
        }

        public bool Setup(ProductionRequirement productionRequirement, int level)
        {
            currentProductionRequirement = productionRequirement;
            currentLevel = level;
            return true;
        }
    }
}