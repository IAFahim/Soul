using Alchemy.Inspector;
using Pancake.Pools;
using QuickEye.Utility;
using Soul.Controller.Runtime.Converters;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Grids;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.Records;
using Soul.Controller.Runtime.Requirements;
using Soul.Controller.Runtime.SpritePopups;
using Soul.Model.Runtime.Containers;
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
        [FormerlySerializedAs("requirementScriptableObject")] [FormerlySerializedAs("basicRequirementScriptableObject")] [SerializeField] private RequirementForUpgrades requirementForUpgrades;
        [SerializeField] private int capacity;
        [SerializeField] private WorkerType basicWorkerType;
        [SerializeField] private ItemToItemConverter itemToItemConverter;

        [SerializeField] private Seed queueItem;
        [SerializeField] private bool isClaimable;

        [FormerlySerializedAs("rewardPopup")] [SerializeField]
        private PopupIndicatorIconCount popupIndicator;
        
        public GridWayPointLimiter wayPointLimiter;


        // Properties
        public Seed ProductionItem
        {
            get
            {
                var seed = recordReference.productionItem as Seed;
                return seed ?? queueItem;
            }
            set => recordReference.productionItem = value;
        }

        public float WeightLimit => capacity;
        public int CurrencyRequirement => 0;
        public int WorkerCount => recordReference.worker.typeAndCount;

        public RequirementForUpgrade Required => requirementForUpgrades.GetRequirement(levelReference - 1);

        public override UnityTimeSpan FullTimeRequirement => ProductionItem.growTime;

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


        public bool Setup(PlayerInventoryReference inventoryReference, RecordProduction recordProduction, Level level,
            ISaveAbleReference saveAbleReference)
        {
            playerInventoryReference = inventoryReference;
            return Setup(recordProduction, level, saveAbleReference);
        }


        /// <summary>
        /// Temporarily adds items for preview purposes.
        /// </summary>
        public void TempAdd(Item[] items)
        {
            queueItem = items[0] as Seed;
            playerInventoryReference.inventoryPreview.AddOrIncrease(queueItem, (int)WeightLimit);
            playerInventoryReference.workerInventoryPreview.TryDecrease(basicWorkerType, Required.workerCount);
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
            recordReference.productionItem = ProductionItem;
            recordReference.Time.Discount = new UnityTimeSpan();
            base.ModifyRecordBeforeProgression();
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
        }

        /// <summary>
        /// Takes the required resources for production.
        /// </summary>
        protected override void TakeRequirement()
        {
            playerInventoryReference.inventory.TryDecrease(ProductionItem, capacity);
        }

        /// <summary>
        /// Adds the reward to the player's inventory.
        /// </summary>
        private void AddReward()
        {
            ModifyRecordAfterProgression();
        }

        private void ModifyRecordAfterProgression()
        {
            var singleReward = Reward;
            playerInventoryReference.inventory.AddOrIncrease(singleReward.Key, singleReward.Value);
            playerInventoryReference.coins.Set(CurrentCurrency + 10);
            recordReference.InProgression = false;
            ProductionItem = null;
            SaveAbleReference.Save();
        }

        private void OnDrawGizmosSelected()
        {
            if (wayPointLimiter == null) return;
            wayPointLimiter.OnDrawGizmosSelected();
        }
    }
}