using System;
using Alchemy.Inspector;
using Pancake;
using Pancake.Common;
using Pancake.Pools;
using QuickEye.Utility;
using Soul.Controller.Runtime.Converters;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.MeshPlanters;
using Soul.Controller.Runtime.Requirements;
using Soul.Controller.Runtime.RequiresAndRewards;
using Soul.Controller.Runtime.Rewards;
using Soul.Controller.Runtime.SpritePopups;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.ParticleEffects;
using Soul.Model.Runtime.Peoples.Workers;
using Soul.Model.Runtime.Productions;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.RequiredAndRewards.Rewards;
using Soul.Model.Runtime.SaveAndLoad;
using UnityEngine;

namespace Soul.Controller.Runtime.Productions
{
    public class CropProductionManager : ProgressionManager<RecordProduction>, IWeightCapacityReference,
        IRewardClaim, IReward<Pair<Item, int>>, ILoadComponent
    {
        [SerializeField] private PlayerInventoryReference playerInventoryReference;
        [SerializeField] private RequiredAndRewardForProductions requiredAndRewardForProductions;
        [SerializeField] private WorkerType basicWorkerType;
        [SerializeField] private ItemToItemConverter itemToItemConverter;

        [SerializeField] private Seed queueItem;
        [SerializeField] private bool isClaimable;
        public MeshPlantPointGridSystem meshPlantPointGridSystem;

        [SerializeField] private PopupIndicatorIconCount popupIndicator;
        [SerializeField] protected Optional<AddressableParticleEffect> particleEffect;


        // Properties
        public Pair<Item, int> ProductionItem
        {
            get
            {
                var itemKeyValuePair = recordReference.productionItemValuePair;
                if (!itemKeyValuePair.Key)
                    return new Pair<Item, int>(queueItem, recordReference.productionItemValuePair.Value);
                return itemKeyValuePair;
            }
            set => recordReference.productionItemValuePair = value;
        }

        public RequirementForProduction Required => requiredAndRewardForProductions.GetRequirement(levelReference - 1);
        public Pair<Currency, int> CurrencyRequirement => Required.currency;

        public int WorkerUsed
        {
            get => recordReference.worker.typeAndCount;
            set => recordReference.worker.typeAndCount.Value = value;
        }

        public int WeightCapacity => Required.weightCapacity;
        public RewardForProduction RewardForProduction => requiredAndRewardForProductions.GetReward(levelReference - 1);

        public int PlantCount =>
            recordReference.productionItemValuePair.Value * ((IKgToCount)ProductionItem.Key).KgToPoint;

        public override UnityTimeSpan FullTimeRequirement => ((ITimeRequirement)ProductionItem.Key).RequiredTime;

        public int CurrentCurrency => playerInventoryReference.coins.Value;

        public Pair<Item, int> Reward
        {
            get
            {
                if (itemToItemConverter.TryConvert(ProductionItem, out var convertedItem))
                {
                    int productionAmount =
                        (int)(convertedItem.Multiplier * recordReference.productionItemValuePair.Value
                                                       * RewardForProduction.productionMultiplier);
                    return new Pair<Item, int>(convertedItem.Key, productionAmount);
                }

                return new Pair<Item, int>();
            }
        }

        public IPlantStageMesh PlantStageMesh => Reward.Key as IPlantStageMesh;

        // Public Methods


        public bool Setup(PlayerInventoryReference inventoryReference,
            IProductionRecordReference<RecordProduction> recordProduction, Level level,
            ISaveAbleReference saveAbleReference)
        {
            playerInventoryReference = inventoryReference;
            return Setup(recordProduction.ProductionRecord, level, saveAbleReference);
        }


        /// <summary>
        /// Temporarily adds items for preview purposes.
        /// </summary>
        public void Add(Seed seed)
        {
            queueItem = seed;
            playerInventoryReference.workerInventoryPreview.TryDecrease(basicWorkerType, Required.workerCount);
        }


        public override bool TryStartProgression()
        {
            return !isClaimable && base.TryStartProgression();
        }

        public override bool HasEnough() =>
            playerInventoryReference.inventory.HasEnough(ProductionItem, WeightCapacity);


        /// <summary>
        /// Checks if the reward can be claimed.
        /// </summary>
        public bool CanClaim => isClaimable;

        /// <summary>
        /// Claims the reward
        /// </summary>
        [Button]
        public void RewardClaim()
        {
            if (!CanClaim) return;
            AddReward();
            isClaimable = false;
        }


        // Private Methods

        /// <summary>
        /// Sets the production record data.
        /// </summary>
        protected override void ModifyRecordBeforeProgression()
        {
            var requiredWorker = Required.workerCount;
            recordReference.worker.typeAndCount = new Pair<WorkerType, int>(basicWorkerType, requiredWorker);
            playerInventoryReference.workerInventory.TryDecrease(basicWorkerType, requiredWorker);
            ProductionItem = new Pair<Item, int>(queueItem, WeightCapacity);
            recordReference.Time.Discount = new UnityTimeSpan();
            base.ModifyRecordBeforeProgression();
        }


        public override void OnTimerStart(bool startsNow)
        {
            if (particleEffect) particleEffect.Value.Load(true, Transform).Forget();
            IPlantStageMesh plantStageMesh = PlantStageMesh;
            meshPlantPointGridSystem.Plant(levelReference, plantStageMesh.StageMeshes[0], plantStageMesh.Size);
        }


        /// <summary>
        /// Called when the production timer completes.
        /// </summary>
        public override void OnComplete()
        {
            isClaimable = true;
            var instantiatedRewardPopup = 
                popupIndicator.gameObject.Request(Transform).GetComponent<PopupIndicatorIconCount>();
            instantiatedRewardPopup.Setup(playerInventoryReference.mainCameraReference.transform, this, this, true);
            meshPlantPointGridSystem.ChangeMesh(PlantStageMesh.StageMeshes[^1]);
            if (particleEffect) particleEffect.Value.Play();
        }

        /// <summary>
        /// Takes the required resources for production.
        /// </summary>
        protected override void TakeRequirement()
        {
            var seed = ProductionItem;
            playerInventoryReference.inventory.TryDecrease(seed, ProductionItem.Value);
        }

        /// <summary>
        /// Adds the reward to the player's inventory.
        /// </summary>
        private void AddReward()
        {
            ModifyRecordAfterProgression();
            meshPlantPointGridSystem.Clear();
            particleEffect.Value.Stop();
        }

        private void ModifyRecordAfterProgression()
        {
            var singleReward = Reward;
            var takenWorker = recordReference.worker.typeAndCount;
            playerInventoryReference.workerInventory.AddOrIncrease(takenWorker.Key, takenWorker.Value);
            playerInventoryReference.inventory.AddOrIncrease(singleReward.Key, singleReward.Value);
            playerInventoryReference.coins.Set(CurrentCurrency + 10);
            recordReference.InProgression = false;
            ProductionItem = new Pair<Item, int>();
            SaveAbleReference.Save();
        }

        void ILoadComponent.OnLoadComponents()
        {
            Reset();
        }

        protected void Reset()
        {
            meshPlantPointGridSystem = GetComponent<MeshPlantPointGridSystem>();
        }
    }
}