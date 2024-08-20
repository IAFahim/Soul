﻿using System;
using Alchemy.Inspector;
using Pancake;
using Pancake.Common;
using Pancake.Pools;
using QuickEye.Utility;
using Soul.Controller.Runtime.Converters;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Controller.Runtime.Rewards;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
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
        [SerializeField] private Level currentLevel;
        [SerializeField] private WorkerType basicWorkerType;
        [SerializeField] private ItemToItemConverter itemToItemConverter;

        [FormerlySerializedAs("currentRecordProduction")] [SerializeField]
        private RecordProduction productionRecord;

        private ISaveAbleReference _saveAbleReference;
        [SerializeField] private Item queueItem;
        [SerializeField] private bool isClaimable;
        private DelayHandle _delayHandle;

        [FormerlySerializedAs("reward")] [SerializeField]
        private RewardPopup rewardPopup;

        // Properties
        public Item ProductionItem => productionRecord.productionItem ??= queueItem;

        public float WeightLimit => capacity;

        public bool IsProducing
        {
            get => productionRecord.isProducing;
            set => productionRecord.isProducing = value;
        }

        public int CurrencyRequirement => 0;
        public int WorkerCount => productionRecord.workerCount;
        public UnityDateTime StartTime => productionRecord.startTime;
        public UnityTimeSpan ReductionTime => productionRecord.reductionTime;

        public WorkerGroupTimeCurrencyRequirement<Item, int> Required =>
            requirementOfWorkerGroupTimeCurrencyForLevels.GetRequirement(currentLevel - 1);

        public TimeSpan RequiredFullTime => Required.fullTime;
        public UnityTimeSpan DiscountedTime => productionRecord.DiscountedTime(Required.fullTime);
        public UnityDateTime EndTime => productionRecord.EndTime(RequiredFullTime);
        public UnityTimeSpan TimeRemaining => productionRecord.TimeRemaining(RequiredFullTime);
        public float Progress => productionRecord.Progress(RequiredFullTime);
        public bool IsCompleted => IsProducing && productionRecord.IsCompleted(RequiredFullTime);


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
        public bool Setup(PlayerInventoryReference inventoryReference, RecordProduction recordProduction, Level level,
            ISaveAbleReference saveAbleReference)
        {
            playerInventoryReference = inventoryReference;
            productionRecord = recordProduction;
            currentLevel = level;
            _saveAbleReference = saveAbleReference;
            if (IsProducing) StartTimer(false);
            return IsProducing;
        }


        /// <summary>
        /// Temporarily adds items for preview purposes.
        /// </summary>
        public void TempAdd(Item[] items)
        {
            queueItem = items[0];
            playerInventoryReference.inventoryPreview.AddOrIncrease(queueItem, (int)WeightLimit);
            playerInventoryReference.workerInventoryPreview.TryDecrease(basicWorkerType, Required.workerCount);
        }

        /// <summary>
        /// Starts the production process.
        /// </summary>
        public bool StartProduction()
        {
            if (IsProducing) return false;
            if (HasEnough())
            {
                ProductionRecordModify();
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
        public bool HasEnough()
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

            float delay = startNow ? (float)DiscountedTime.TotalSeconds : (float)TimeRemaining.TotalSeconds;
            _delayHandle = App.Delay(delay, OnComplete);
            Track.Start(name, delay);
        }

        /// <summary>
        /// Sets the production record data.
        /// </summary>
        private void ProductionRecordModify()
        {
            IsProducing = true;
            productionRecord.workerCount.Value = WorkerCount;
            productionRecord.productionItem = ProductionItem;
            productionRecord.startTime = new UnityDateTime(DateTime.UtcNow);
            productionRecord.reductionTime = new UnityTimeSpan();
        }

        /// <summary>
        /// Called when the production timer completes.
        /// </summary>
        private void OnComplete()
        {
            isClaimable = true;
            var instantiatedRewardPopup = rewardPopup.gameObject.Request(Transform).GetComponent<RewardPopup>();
            instantiatedRewardPopup.Setup(this, this, true);
        }

        /// <summary>
        /// Takes the required resources for production.
        /// </summary>
        private void TakeRequirement()
        {
            playerInventoryReference.inventory.TryDecrease(ProductionItem, capacity);
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