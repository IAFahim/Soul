using Alchemy.Inspector;
using Pancake.Pools;
using QuickEye.Utility;
using Soul.Controller.Runtime.Converters;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.MeshPlanters;
using Soul.Controller.Runtime.Records;
using Soul.Controller.Runtime.Requirements;
using Soul.Controller.Runtime.RequiresAndRewards;
using Soul.Controller.Runtime.Rewards;
using Soul.Controller.Runtime.SpritePopups;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Peoples.Workers;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.Rewards;
using Soul.Model.Runtime.SaveAndLoad;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Buildings.Managers
{
    public class CropProductionManager : ProgressionManager<RecordProduction>, ISingleDrop, IWeightCapacity,
        IRewardClaim,
        IReward<Pair<Item, int>>
    {
        [SerializeField] private PlayerInventoryReference playerInventoryReference;
        [SerializeField] private RequiredAndRewardForProductions requiredAndRewardForProductions;
        [SerializeField] private WorkerType basicWorkerType;
        [SerializeField] private ItemToItemConverter itemToItemConverter;

        [SerializeField] private Pair<Item, int> queueItemValuePair;
        [SerializeField] private bool isClaimable;

        [SerializeField] private int capacity = 3;
        [SerializeField] private PopupIndicatorIconCount popupIndicator;

        [FormerlySerializedAs("meshPlantGridSystem")]
        public MeshPlantPointGridSystem meshPlantPointGridSystem;


        // Properties
        public Pair<Item, int> ProductionItem
        {
            get
            {
                var itemKeyValuePair = recordReference.productionItemValuePair;
                if (!itemKeyValuePair.Key) return queueItemValuePair;
                return itemKeyValuePair;
            }
            set => recordReference.productionItemValuePair = value;
        }

        public float WeightLimit => capacity;
        public int CurrencyRequirement => 0;
        public int WorkerCount => recordReference.worker.typeAndCount;

        public RequirementForProduction Required => requiredAndRewardForProductions.GetRequirement(levelReference - 1);
        public RewardForProduction RewardForProduction => requiredAndRewardForProductions.GetReward(levelReference - 1);

        public int PlantCount => capacity * ((IKgToCount)ProductionItem.Key).KgToPoint;

        public override UnityTimeSpan FullTimeRequirement => ((ITimeRequirement)ProductionItem.Key).RequiredTime;

        public int CurrentCurrency => playerInventoryReference.coins.Value;

        public Pair<Item, int> Reward
        {
            get
            {
                if (itemToItemConverter.TryConvert(ProductionItem, out var convertedItem))
                {
                    int productionAmount =
                        (int)(convertedItem.Value * capacity * RewardForProduction.productionMultiplier);
                    return new Pair<Item, int>(convertedItem.Key, productionAmount);
                }

                return new Pair<Item, int>();
            }
        }

        public IPlantStageMesh PlantStageMesh => Reward.Key as IPlantStageMesh;

        // Public Methods


        public bool Setup(PlayerInventoryReference inventoryReference, RecordProduction recordProduction, Level level,
            ISaveAbleReference saveAbleReference)
        {
            playerInventoryReference = inventoryReference;
            return Setup(recordProduction, level, saveAbleReference);
        }


        /// <summary>
        /// Temporarily adds items for preview purposes.
        /// </summary>
        public void TempAdd(Pair<Item, int> itemKeyValuePair)
        {
            queueItemValuePair = itemKeyValuePair; 
            playerInventoryReference.inventoryPreview.AddOrIncrease(queueItemValuePair, (int)WeightLimit);
            playerInventoryReference.workerInventoryPreview.TryDecrease(basicWorkerType, Required.workerCount);
        }

        

        public override bool TryStartProgression()
        {
            return !isClaimable && base.TryStartProgression();
        }


        public override bool HasEnough() => playerInventoryReference.inventory.HasEnough(ProductionItem, capacity);


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
            recordReference.worker.Set(WorkerCount);
            recordReference.productionItemValuePair = ProductionItem;
            recordReference.Time.Discount = new UnityTimeSpan();
            base.ModifyRecordBeforeProgression();
        }


        public override void OnTimerStart()
        {
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
            instantiatedRewardPopup.Setup(this, this, true);
            meshPlantPointGridSystem.ChangeMesh(PlantStageMesh.StageMeshes[^1]);
        }

        /// <summary>
        /// Takes the required resources for production.
        /// </summary>
        protected override void TakeRequirement()
        {
            var seed = ProductionItem;
            playerInventoryReference.inventory.TryDecrease(seed, capacity);
        }

        /// <summary>
        /// Adds the reward to the player's inventory.
        /// </summary>
        private void AddReward()
        {
            ModifyRecordAfterProgression();
            meshPlantPointGridSystem.Clear();
        }

        private void ModifyRecordAfterProgression()
        {
            var singleReward = Reward;
            playerInventoryReference.inventory.AddOrIncrease(singleReward.Key, singleReward.Value);
            playerInventoryReference.coins.Set(CurrentCurrency + 10);
            recordReference.InProgression = false;
            ProductionItem = new Pair<Item, int>();
            SaveAbleReference.Save();
        }
    }
}