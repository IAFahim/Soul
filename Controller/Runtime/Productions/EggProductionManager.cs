using Pancake;
using Pancake.Pools;
using QuickEye.Utility;
using Soul.Controller.Runtime.Buildings;
using Soul.Controller.Runtime.Converters;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.Requirements;
using Soul.Controller.Runtime.RequiresAndRewards;
using Soul.Controller.Runtime.SpritePopups;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Limits;
using Soul.Model.Runtime.ParticleEffects;
using Soul.Model.Runtime.Peoples.Workers;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.RequiredAndRewards.Rewards;
using Soul.Model.Runtime.SaveAndLoad;
using UnityEngine;

namespace Soul.Controller.Runtime.Productions
{
    public class EggProductionManager : ProgressionManager<RecordProduction>, IWeightCapacityReference,
        IRewardClaim,
        IReward<Pair<Item, int>>
    {
        [SerializeField] private Limit weightLimit;
        [SerializeField] private PlayerInventoryReference playerInventoryReference;
        [SerializeField] private RequiredAndRewardForProductions requiredAndRewardForProductions;
        [SerializeField] private WorkerType basicWorkerType;
        [SerializeField] private ItemToItemConverter itemToItemConverter;
        [SerializeField] private bool isClaimable;
        [SerializeField] private PopupIndicatorIconCount popupIndicator;
        [SerializeField] protected Optional<AddressableParticleEffect> particleEffect;


        public override UnityTimeSpan FullTimeRequirement => UnityTimeSpan.MinValue;

        protected override bool Setup(RecordProduction record, Level level, ISaveAbleReference saveAbleReference)
        {
            bool canStart = base.Setup(record, level, saveAbleReference);
            weightLimit.currentAndMax = new Vector2Int(record.productionItemValuePair.Value, Required.weightCapacity);
            return canStart;
        }
        
        public RequirementForProduction Required => requiredAndRewardForProductions.GetRequirement(levelReference - 1);


        public Pair<Item, int> ProductionItemValuePair
        {
            get => recordReference.productionItemValuePair;
            set => recordReference.productionItemValuePair = new Pair<Item, int>(value.Key, value.Value);
        }

        protected override void TakeRequirement()
        {
            playerInventoryReference.workerInventory.TryDecrease(basicWorkerType, 1);
        }

        public override bool HasEnough()
        {
            return playerInventoryReference.workerInventory.HasEnough(basicWorkerType, 1);
        }

        public override void OnTimerStart(bool startsNow)
        {
            // TODO: move worker here
        }

        public override void OnComplete()
        {
            isClaimable = true;
            var instantiatedRewardPopup =
                popupIndicator.gameObject.Request(Transform).GetComponent<PopupIndicatorIconCount>();
            instantiatedRewardPopup.Setup(playerInventoryReference.mainCameraReference.transform, this, this, true);
        }

        public int WeightCapacity => requiredAndRewardForProductions.GetRequirement(levelReference - 1).weightCapacity;
        public bool CanClaim => isClaimable;

        public void RewardClaim()
        {
            if (CanClaim)
            {
                
            }
        }

        public bool IsMaxed => weightLimit.IsMax;

        public Pair<Item, int> Reward => ProductionItemValuePair;
    }
}