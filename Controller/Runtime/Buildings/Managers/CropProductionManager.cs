using System;
using Pancake;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Controller.Runtime.Converters;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Peoples.Workers;
using Soul.Model.Runtime.Requirements;
using Soul.Model.Runtime.SaveAndLoad;
using TrackTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Buildings.Managers
{
    public class CropProductionManager : GameComponent, ISingleDrop, IWeightCapacity
    {
        public PlayerInventoryReference playerInventoryReference;

        [FormerlySerializedAs("currentProductionRequirement")]
        public RecordProduction currentRecordProduction;

        [SerializeField]
        private RequirementOfWorkerGroupTimeCurrencyForLevels requirementOfWorkerGroupTimeCurrencyForLevels;

        public int capacity;
        public int currentLevel;
        public WorkerType basicWorkerType;
        public ItemToItemConverter itemToItemConverter;
        public Item queueItem;

        public DelayHandle delayHandle;
        private ISaveAbleReference _saveAbleReference;

        public Item ProductionItem
        {
            get
            {
                if (currentRecordProduction.productionItem == null)
                {
                    currentRecordProduction.productionItem = queueItem;
                }

                return currentRecordProduction.productionItem;
            }
        }

        public float WeightLimit => capacity;

        public bool IsProducing
        {
            get => currentRecordProduction.isProducing;
            set => currentRecordProduction.isProducing = value;
        }

        public int CurrencyRequirement => 0;
        public int WorkerCount => currentRecordProduction.workerCount.Value;
        public UnityDateTime StartTime => currentRecordProduction.startTime;
        public UnityTimeSpan ReductionTime => currentRecordProduction.reductionTime;
        public UnityTimeSpan RequiredTimeSpan => Required.time - ReductionTime;
        public UnityDateTime EndTime => StartTime + RequiredTimeSpan;
        public UnityTimeSpan TimeLeft => EndTime - DateTime.UtcNow;
        public bool IsCompleted => IsProducing && TimeLeft <= TimeSpan.Zero;

        public WorkerGroupTimeCurrencyRequirement<Item, int> Required =>
            requirementOfWorkerGroupTimeCurrencyForLevels.GetRequirement(currentLevel);

        public TimeSpan TimeRequired => Required.time;
        public int CurrentCurrency => playerInventoryReference.coins.Value;

        public bool Setup(RecordProduction recordProduction, int level, ISaveAbleReference saveAbleReference)
        {
            currentRecordProduction = recordProduction;
            currentLevel = level;
            _saveAbleReference = saveAbleReference;
            if (currentRecordProduction.isProducing)
            {
                StartTimer(false);
            }

            return true;
        }

        public void TempAdd(Item[] items)
        {
            queueItem = items[0];
            playerInventoryReference.inventoryPreview.AddOrIncrease(queueItem, (int)WeightLimit);
            playerInventoryReference.workerInventoryPreview.Decrease(basicWorkerType, Required.workerCount);
        }

        public bool StartProduction()
        {
            if (IsProducing) return false;
            if (HasEnoughToStart())
            {
                SetProductionRecord();
                TakeRequirement();
                StartTimer(true);
                _saveAbleReference.Save();
                return true;
            }

            return false;
        }

        public void StartTimer(bool startNow)
        {
            if (startNow)
            {
                delayHandle = App.Delay((float)RequiredTimeSpan.TotalSeconds, OnComplete);
                Track.Start(name, (float)RequiredTimeSpan.TotalSeconds);
            }
            else
            {
                delayHandle = App.Delay((float)TimeLeft.TotalSeconds, OnComplete);
                Track.Start(name, (float)TimeLeft.TotalSeconds);
            }
        }

        private void SetProductionRecord()
        {
            IsProducing = true;
            currentRecordProduction.workerCount.Value = WorkerCount;
            currentRecordProduction.productionItem = ProductionItem;
            currentRecordProduction.startTime = new UnityDateTime(DateTime.UtcNow);
            currentRecordProduction.reductionTime = new UnityTimeSpan();
        }

        public void OnComplete()
        {
            AddReward();
        }
        
        public void TakeRequirement()
        {
            playerInventoryReference.inventory.Decrease(ProductionItem, capacity);
        }

        private void AddReward()
        {
            if (itemToItemConverter.TryConvert(ProductionItem, out var convertedItem))
            {
                int productionAmount = (int)(convertedItem.Value * capacity);
                playerInventoryReference.inventory.AddOrIncrease(convertedItem.Key, productionAmount);
            }

            playerInventoryReference.coins.Set(CurrentCurrency + 10);
            IsProducing = false;
            _saveAbleReference.Save();
        }

        public bool HasEnoughToStart()
        {
            return playerInventoryReference.inventory.HasEnough(ProductionItem, capacity);
        }
    }
}