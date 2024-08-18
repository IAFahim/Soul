using System;
using System.Security.Cryptography.X509Certificates;
using Alchemy.Inspector;
using Pancake;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Controller.Runtime.Converters;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Peoples.Workers;
using Soul.Model.Runtime.Requirements;
using Soul.Model.Runtime.Rewards;
using Soul.Model.Runtime.SaveAndLoad;
using TrackTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Buildings.Managers
{
    public class CropProductionManager : GameComponent, ISingleDrop, IWeightCapacity, IRewardClaim,
        IRewardSingle<Item, int>
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

        public bool isClaimable;
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

        public float Progress => 1 - (float)TimeLeft.TotalSeconds / (float)RequiredTimeSpan.TotalSeconds;
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
            if (IsCompleted)
            {
                OnComplete();
                return;
            }

            if (startNow)
            {
                delayHandle = App.Delay((float)RequiredTimeSpan.TotalSeconds, OnComplete);
            }
            else
            {
                delayHandle = App.Delay((float)TimeLeft.TotalSeconds, OnComplete);
            }

            Track.Start(name, (float)RequiredTimeSpan.TotalSeconds);
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
            isClaimable = true;
        }

        public void TakeRequirement()
        {
            playerInventoryReference.inventory.Decrease(ProductionItem, capacity);
        }

        private void AddReward()
        {
            var singleReward = RewardSingleItem;
            playerInventoryReference.inventory.AddOrIncrease(singleReward.Key, singleReward.Value);
            playerInventoryReference.coins.Set(CurrentCurrency + 10);
            IsProducing = false;
            _saveAbleReference.Save();
        }

        public bool HasEnoughToStart()
        {
            return playerInventoryReference.inventory.HasEnough(ProductionItem, capacity);
        }

        public bool CanClaim() => isClaimable;

        [Button]
        public void RewardClaim()
        {
            if (!isClaimable) return;
            AddReward();
            isClaimable = false;
        }

        public Pair<Item, int> RewardSingleItem
        {
            get
            {
                if (itemToItemConverter.TryConvert(ProductionItem, out var convertedItem))
                {
                    int productionAmount = (int)(convertedItem.Value * capacity);
                    return new Pair<Item, int>(convertedItem.Key, productionAmount);
                }

                return new Pair<Item, int>();
            }
        }
    }
}