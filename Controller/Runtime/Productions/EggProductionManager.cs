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
        [SerializeField] private PlayerInventoryReference playerInventoryReference;
        [SerializeField] private RequiredAndRewardForProductions requiredAndRewardForProductions;
        [SerializeField] private WorkerType basicWorkerType;
        [SerializeField] private ItemToItemConverter itemToItemConverter;
        [SerializeField] private bool isClaimable;

        [FormerlySerializedAs("popupIndicator")] [SerializeField]
        private PopupIndicatorIconCount popupIndicatorPrefab;

        [SerializeField] protected Optional<AddressableParticleEffect> particleEffect;
        private PopupIndicatorIconCount _popupIndicatorInstance;


        public override UnityTimeSpan FullTimeRequirement =>
            itemToItemConverter.Convert(ProductionItemValuePair.Key).timeRequired;

        public bool Setup(Transform parentTransform, PlayerInventoryReference inventoryReference,
            RecordProduction record, Level level,
            ISaveAbleReference saveAbleReference)
        {
            parent = parentTransform;
            playerInventoryReference = inventoryReference;
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
            if (_popupIndicatorInstance == null)
            {
                _popupIndicatorInstance = popupIndicatorPrefab.gameObject.Request(parent)
                    .GetComponent<PopupIndicatorIconCount>();
                _popupIndicatorInstance.Setup(playerInventoryReference.mainCameraReference.transform, this, this,
                    false);
            }
            else
            {
                if (!_popupIndicatorInstance.GameObject.activeSelf) _popupIndicatorInstance.gameObject.SetActive(true);
                _popupIndicatorInstance.Reload();
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
            playerInventoryReference.inventory.AddOrIncrease(ProductionItemValuePair.Key,
                ProductionItemValuePair.Value);
            _popupIndicatorInstance.gameObject.SetActive(false);
        }


        public Pair<Item, int> Reward => ProductionItemValuePair;

        public override string ToString()
        {
            return $"{parent.name}: {ProductionItemValuePair.Key} {ProductionItemValuePair.Value}";
        }
    }
}