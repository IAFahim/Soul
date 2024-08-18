using System;
using Alchemy.Inspector;
using Pancake;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Controller.Runtime.Converters;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Controller.Runtime.Rewards;
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
        IReward<Pair<Item, int>>
    {
        
        [SerializeField] private PlayerInventoryReference playerInventoryReference;

        [SerializeField]
        private RequirementOfWorkerGroupTimeCurrencyForLevels requirementOfWorkerGroupTimeCurrencyForLevels;

        [SerializeField] private int capacity;
        [SerializeField] private int currentLevel;
        [SerializeField] private WorkerType basicWorkerType;
        [SerializeField] private ItemToItemConverter itemToItemConverter;
        
        [SerializeField] private RecordProduction currentRecordProduction;
        private ISaveAbleReference _saveAbleReference;
        [SerializeField] private Item queueItem;
        [SerializeField] private bool isClaimable;
        private DelayHandle _delayHandle;
        
        [FormerlySerializedAs("reward")] [SerializeField] private RewardPopup rewardPopup;

        // Properties
        public Item ProductionItem => currentRecordProduction.productionItem ??= queueItem;

        public float WeightLimit => capacity;

        public bool IsProducing
        {
            get => currentRecordProduction.isProducing;
            set => currentRecordProduction.isProducing = value;
        }

        public int CurrencyRequirement => 0;
        public int WorkerCount => currentRecordProduction.workerCount;
        public UnityDateTime StartTime => currentRecordProduction.startTime;
        public UnityTimeSpan ReductionTime => currentRecordProduction.reductionTime;

        public WorkerGroupTimeCurrencyRequirement<Item, int> Required =>
            requirementOfWorkerGroupTimeCurrencyForLevels.GetRequirement(currentLevel);

        public TimeSpan TimeRequired => Required.time;
        public UnityTimeSpan RequiredTimeSpan => currentRecordProduction.RequiredTime(TimeRequired);
        public UnityDateTime EndTime => currentRecordProduction.EndTime(TimeRequired);
        public UnityTimeSpan TimeRemaining => currentRecordProduction.TimeRemaining(TimeRequired);
        public float Progress => currentRecordProduction.Progress(TimeRequired);
        public bool IsCompleted => IsProducing && currentRecordProduction.IsCompleted(TimeRequired);


        public int CurrentCurrency => playerInventoryReference.coins.Value;

        public Pair<Item, int> Reward
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

        // Public Methods

        /// <summary>
        /// Initializes the CropProductionManager with the provided data.
        /// </summary>
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

        /// <summary>
        /// Temporarily adds items for preview purposes.
        /// </summary>
        public void TempAdd(Item[] items)
        {
            queueItem = items[0];
            playerInventoryReference.inventoryPreview.AddOrIncrease(queueItem, (int)WeightLimit);
            playerInventoryReference.workerInventoryPreview.Decrease(basicWorkerType, Required.workerCount);
        }

        /// <summary>
        /// Starts the production process.
        /// </summary>
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

        /// <summary>
        /// Checks if there are enough resources to start production.
        /// </summary>
        public bool HasEnoughToStart()
        {
            return playerInventoryReference.inventory.HasEnough(ProductionItem, capacity);
        }

        /// <summary>
        /// Checks if the reward can be claimed.
        /// </summary>
        public bool CanClaim => isClaimable;

        /// <summary>
        /// Claims the reward.
        /// </summary>
        [Button]
        public void RewardClaim()
        {
            if (!isClaimable) return;
            AddReward();
            isClaimable = false;
        }


        // Private Methods

        /// <summary>
        /// Starts the production timer.
        /// </summary>
        private void StartTimer(bool startNow)
        {
            if (IsCompleted)
            {
                OnComplete();
                return;
            }

            float delay = startNow ? (float)RequiredTimeSpan.TotalSeconds : (float)TimeRemaining.TotalSeconds;
            _delayHandle = App.Delay(delay, OnComplete);
            Track.Start(name, delay);
        }

        /// <summary>
        /// Sets the production record data.
        /// </summary>
        private void SetProductionRecord()
        {
            IsProducing = true;
            currentRecordProduction.workerCount.Value = WorkerCount;
            currentRecordProduction.productionItem = ProductionItem;
            currentRecordProduction.startTime = new UnityDateTime(DateTime.UtcNow);
            currentRecordProduction.reductionTime = new UnityTimeSpan();
        }

        /// <summary>
        /// Called when the production timer completes.
        /// </summary>
        private void OnComplete()
        {
            isClaimable = true;
            rewardPopup.Setup(this, this, true);
        }

        /// <summary>
        /// Takes the required resources for production.
        /// </summary>
        private void TakeRequirement()
        {
            playerInventoryReference.inventory.Decrease(ProductionItem, capacity);
        }

        /// <summary>
        /// Adds the reward to the player's inventory.
        /// </summary>
        private void AddReward()
        {
            var singleReward = Reward;
            playerInventoryReference.inventory.AddOrIncrease(singleReward.Key, singleReward.Value);
            playerInventoryReference.coins.Set(CurrentCurrency + 10);
            IsProducing = false;
            _saveAbleReference.Save();
        }
    }
}