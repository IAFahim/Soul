using System;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
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
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.ParticleEffects;
using Soul.Model.Runtime.Peoples.Workers;
using Soul.Model.Runtime.Productions;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.RequiredAndRewards.Rewards;
using Soul.Model.Runtime.SaveAndLoad;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Productions
{
    [Serializable]
    public class CropProductionManager : ProgressionManager<RecordProduction>, IWeightCapacityReference,
        IRewardClaim, IReward<Pair<Item, int>>
    {
        // Serialized Fields
        [SerializeField] private RequiredAndRewardForProductions requiredAndRewardForProductions;
        [SerializeField] private WorkerType basicWorkerType;
        [SerializeField] private ItemToItemConverter itemToItemConverter;
        [SerializeField] private Seed queueItem;
        [FormerlySerializedAs("isClaimable")] [SerializeField] private bool canClaim;
        [SerializeField] public MeshPlantPointGridSystem meshPlantPointGridSystem;
        [SerializeField] private PopupIndicatorIconCount popupIndicator;
        private Transform _parent;
        private PlayerInventoryReference _playerInventoryReference;
        [SerializeField] protected Optional<AddressableParticleEffect> onCompleteParticleEffect;


        // Properties
        public Pair<Item, int> ProductionItemValuePair
        {
            get
            {
                var itemKeyValuePair = recordReference.productionItemValuePair;
                if (itemKeyValuePair.Key == null)
                {
                    return new Pair<Item, int>(queueItem, recordReference.productionItemValuePair.Value);
                }

                return itemKeyValuePair;
            }
            set => recordReference.productionItemValuePair = value;
        }

        public RequirementForProduction Required => requiredAndRewardForProductions.GetRequirement(LevelReference - 1);
        public Pair<Currency, int> CurrencyRequirement => Required.currency;

        public int WorkerUsed
        {
            get => recordReference.worker.typeAndCount;
            set => recordReference.worker.typeAndCount.Value = value;
        }

        public int WeightCapacity => Required.weightCapacity;
        public RewardForProduction RewardForProduction => requiredAndRewardForProductions.GetReward(LevelReference - 1);

        public int PlantCount => ProductionItemValuePair.Value * ((IKgToCount)ProductionItemValuePair.Key).KgToPoint;

        public override UnityTimeSpan FullTimeRequirement =>
            itemToItemConverter.Convert(ProductionItemValuePair.Key).RequiredTime * Required.timeMultiplier;

        public int CurrentCurrency => _playerInventoryReference.coins.Value;

        public Pair<Item, int> Reward
        {
            get
            {
                if (itemToItemConverter.TryConvert(ProductionItemValuePair, out var convertedItem))
                {
                    int productionAmount = (int)(convertedItem.ratio * recordReference.productionItemValuePair.Value *
                                                 RewardForProduction.productionMultiplier
                        );
                    return new Pair<Item, int>(convertedItem.data, productionAmount);
                }

                return new Pair<Item, int>();
            }
        }


        // Public Methods

        /// <summary>
        /// Initializes the CropProductionManager with the given parameters.
        /// </summary>
        public bool Setup(Transform parentTransform, PlayerInventoryReference inventoryReference,
            IProductionRecordReference<RecordProduction> recordProduction, Level level,
            ISaveAbleReference saveAbleReference)
        {
            _parent = parentTransform;
            _playerInventoryReference = inventoryReference;
            return Setup(recordProduction.ProductionRecord, level, saveAbleReference);
        }

        /// <summary>
        /// Temporarily adds items for preview purposes.
        /// </summary>
        public void Add(Seed seed)
        {
            queueItem = seed;
            _playerInventoryReference.workerInventoryPreview.TryDecrease(basicWorkerType, Required.workerCount);
        }

        /// <summary>
        /// Tries to start the progression, overriding the base method to check the 'isClaimable' flag.
        /// </summary>
        public override bool TryStartProgression()
        {
            return !recordReference.InProgression && !CanClaim && base.TryStartProgression();
        }

        /// <summary>
        /// Checks if there are enough resources to start the progression.
        /// </summary>
        public override bool HasEnough() =>
            _playerInventoryReference.inventory.HasEnough(ProductionItemValuePair, WeightCapacity);

        /// <summary>
        /// Checks if the reward can be claimed.
        /// </summary>
        public bool CanClaim
        {
            get => canClaim;
            set => canClaim = value;
        }


        /// <summary>
        /// Claims the reward if it can be claimed.
        /// </summary>
        [Button]
        public void RewardClaim()
        {
            if (!CanClaim) return;
            AddReward().Forget();
        }


        // Private Methods

        /// <summary>
        /// Modifies the record data before the progression starts.
        /// </summary>
        protected override void ModifyRecordBeforeProgression()
        {
            var requiredWorker = Required.workerCount;
            recordReference.worker.typeAndCount = new Pair<WorkerType, int>(basicWorkerType, requiredWorker);
            _playerInventoryReference.workerInventory.TryDecrease(basicWorkerType, requiredWorker);
            ProductionItemValuePair = new Pair<Item, int>(queueItem, WeightCapacity);
            recordReference.Time.Discount = new UnityTimeSpan();
            base.ModifyRecordBeforeProgression();
        }

        /// <summary>
        /// Called when the progression timer starts.
        /// </summary>
        public override void OnTimerStart(float progressRatio)
        {
            meshPlantPointGridSystem.Setup(LevelReference, Reward.Key, progressRatio);
        }

        /// <summary>
        /// Called when the progression timer completes.
        /// </summary>
        public override void OnComplete()
        {
            if (onCompleteParticleEffect) onCompleteParticleEffect.Value.Load(true, _parent).Forget();
            CanClaim = true;
            var instantiatedRewardPopup =
                popupIndicator.gameObject.Request(_parent).GetComponent<PopupIndicatorIconCount>();
            instantiatedRewardPopup.Setup(_playerInventoryReference.mainCameraReference.transform, this, this, true);
            meshPlantPointGridSystem.Complete();
        }

        /// <summary>
        /// Takes the required resources for the progression.
        /// </summary>
        protected override void TakeRequirement()
        {
            var seed = ProductionItemValuePair;
            _playerInventoryReference.inventory.TryDecrease(seed, ProductionItemValuePair.Value);
        }

        /// <summary>
        /// Adds the reward to the player's inventory.
        /// </summary>
        private async UniTaskVoid AddReward()
        {
            await meshPlantPointGridSystem.ClearAsync();
            ModifyRecordAfterProgression();
            onCompleteParticleEffect.Value.Stop();
            CanClaim = false;
        }

        /// <summary>
        /// Modifies the record data after the progression completes.
        /// </summary>
        private void ModifyRecordAfterProgression()
        {
            var singleReward = Reward;
            var takenWorker = recordReference.worker.typeAndCount;
            _playerInventoryReference.workerInventory.AddOrIncrease(takenWorker.Key, takenWorker.Value);
            _playerInventoryReference.inventory.AddOrIncrease(singleReward.Key, singleReward.Value);
            _playerInventoryReference.coins.Set(CurrentCurrency + 10);
            recordReference.InProgression = false;
            ProductionItemValuePair = new Pair<Item, int>(queueItem, WeightCapacity);
            SaveAbleReference.Save();
        }

        public override string ToString()
        {
            return $"{_parent.name}: {ProductionItemValuePair.Key} x {ProductionItemValuePair.Value}";
        }
    }
}