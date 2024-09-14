using System;
using Pancake;
using Pancake.Pools;
using QuickEye.Utility;
using Soul.Controller.Runtime.Converters;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Controller.Runtime.RequiresAndRewards;
using Soul.Controller.Runtime.SpritePopups;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.ParticleEffects;
using Soul.Model.Runtime.Peoples.Workers;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.RequiredAndRewards.Rewards;
using Soul.Model.Runtime.SaveAndLoad;
using UnityEngine;
using UnityEngine.Serialization;
using Math = Pancake.Common.Math;

namespace Soul.Controller.Runtime.Productions
{
    [Serializable]
    public class EggProductionManager : ProgressionManager<RecordProduction>, IWeightCapacityReference,
        IRewardClaim,
        IReward<Pair<Item, int>>
    {
        [SerializeField] private Transform parent;
        [FormerlySerializedAs("playerInventoryReference")] [SerializeField] private PlayerFarmReference playerFarmReference;
        [SerializeField] private RequiredAndRewardForProductions requiredAndRewardForProductions;
        [SerializeField] private WorkerType basicWorkerType;
        [SerializeField] private ItemToItemConverter itemToItemConverter;
        [SerializeField] private bool isClaimable;
        
        [FormerlySerializedAs("popupClickableIndicatorPrefab")] [FormerlySerializedAs("popupIndicatorPrefab")] [FormerlySerializedAs("popupIndicator")] [SerializeField]
        private PopupClickableIconCount popupClickablePrefab;

        [SerializeField] protected Optional<AddressableParticleEffect> particleEffect;
        private PopupClickableIconCount popupClickableInstance;


        public override UnityTimeSpan FullTimeRequirement =>
            itemToItemConverter.Convert(ProductionItemValuePair.Key).timeRequired;

        public bool Setup(Transform parentTransform, PlayerFarmReference farmReference,
            RecordProduction record, Level level,
            ISaveAbleReference saveAbleReference)
        {
            parent = parentTransform;
            playerFarmReference = farmReference;
            record.InProgression = true;
            bool canStart = base.Setup(record, level, saveAbleReference);
            if (!canStart) return false;
            return true;
        }

        public RequirementForProduction RequiredLimit =>
            requiredAndRewardForProductions.GetRequirement(LevelReference - 1);


        public Pair<Item, int> ProductionItemValuePair
        {
            get => recordReference.productionItemValuePair;
            set => recordReference.productionItemValuePair = new Pair<Item, int>(value.Key, value.Value);
        }

        protected override void TakeRequirement()
        {
        }

        public override bool HasEnough()
        {
            return ProductionItemValuePair.Value < RequiredLimit.weightCapacity;
        }

        public override void OnTimerStart(float progressRatio)
        {
        }

        public override void OnComplete()
        {
            isClaimable = true;
            int currentClamped = Math.Clamp(ProductionItemValuePair.Value + 1, 1, RequiredLimit.weightCapacity);
            ProductionItemValuePair = new Pair<Item, int>(ProductionItemValuePair.Key, currentClamped);
            if (popupClickableInstance == null)
            {
                popupClickableInstance = popupClickablePrefab.gameObject.Request(parent)
                    .GetComponent<PopupClickableIconCount>();
                popupClickableInstance.Setup(this, this,
                    false);
            }
            else
            {
                if (!popupClickableInstance.GameObject.activeSelf) popupClickableInstance.gameObject.SetActive(true);
                popupClickableInstance.Reload();
            }

            TryStartTimer();
        }


        private void TryStartTimer()
        {
            if (HasEnough())
            {
                ModifyRecordBeforeProgression();
                StartTimer(true);
            }
        }

        public int WeightCapacity => requiredAndRewardForProductions.GetRequirement(LevelReference - 1).weightCapacity;
        public bool CanClaim => isClaimable;

        public void RewardClaim()
        {
            if (CanClaim)
            {
                AddReward();
                isClaimable = false;
                ProductionItemValuePair = new Pair<Item, int>(ProductionItemValuePair.Key, 0);
                DelayHandle?.Cancel();
                TryStartTimer();
            }
        }

        private void AddReward()
        {
            playerFarmReference.inventory.AddOrIncrease(ProductionItemValuePair.Key,
                ProductionItemValuePair.Value);
            popupClickableInstance.gameObject.SetActive(false);
        }


        public Pair<Item, int> Reward => ProductionItemValuePair;

        public override string ToString()
        {
            return $"{parent.name}: {ProductionItemValuePair.Key} {ProductionItemValuePair.Value}";
        }
    }
}