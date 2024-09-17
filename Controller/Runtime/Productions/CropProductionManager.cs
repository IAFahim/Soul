using System;
using _Root.Scripts.NPC_Ai.Runtime;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Links.Runtime;
using Pancake;
using Pancake.Pools;
using QuickEye.Utility;
using Soul.Controller.Runtime.Converters;
using Soul.Controller.Runtime.Indicators;
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

namespace Soul.Controller.Runtime.Productions
{
    [Serializable]
    public class CropProductionManager : ProgressionManager<RecordProduction>, IWeightCapacityReference,
        IRewardClaim, IReward<Pair<Item, int>>
    {
        // Serialized Fields
        public RequiredAndRewardForProductions requiredAndRewardForProductions;
        [SerializeField] private WorkerType basicWorkerType;
        [SerializeField] private ItemToItemConverter itemToItemConverter;
        [SerializeField] private Seed queueItem;

        [SerializeField] private bool canClaim;
        [SerializeField] public MeshPlantPointGridSystem meshPlantPointGridSystem;

        [SerializeField] private PopupClickableIconCount popupClickable;
        [SerializeField] private float height = 10;
        [SerializeField] private IndicatorProgressCapacity indicatorProgressCapacityPrefab;

        [SerializeField] protected Optional<AddressableParticleEffect> onCompleteParticleEffect;
        [SerializeField] ScriptableEventGetGameObject farmerSpawnerGetGameObject;
        
        private bool _isLoaded;
        private Transform _parent;
        private IndicatorProgressCapacity _indicatorProgressCapacity;
        private PlayerFarmReference playerFarmReference;
        
        private FarmerSpawner _farmerSpawner;

        private PopupClickableIconCount _instantiatedRewardPopup;

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


        public int WeightCapacity => Required.weightCapacity;
        public RewardForProduction RewardForProduction => requiredAndRewardForProductions.GetReward(LevelReference - 1);


        public override UnityTimeSpan FullTimeRequirement =>
            itemToItemConverter.Convert(ProductionItemValuePair.Key).RequiredTime * Required.timeMultiplier;

        public int CurrentCurrency => playerFarmReference.coins.Value;

        #region Rewrad

        public Pair<Item, int> Reward => GetReward(itemToItemConverter.Convert(ProductionItemValuePair.Key));

        private Pair<Item, int> GetReward(ConvertTimedInfo<Item, int> convertInfo)
        {
            int productionAmount = (int)(convertInfo.ratio * recordReference.productionItemValuePair.Value *
                                         RewardForProduction.productionMultiplier
                );
            return new Pair<Item, int>(convertInfo.data, productionAmount);
        }

        public ConvertTimedInfo<Item, int> ConvertInfo => itemToItemConverter.Convert(ProductionItemValuePair.Key);
        public int XpTotal => itemToItemConverter.Convert(ProductionItemValuePair.Key).GetXpFrom(WeightCapacity);

        #endregion

        // Public Methods

        /// <summary>
        /// Initializes the CropProductionManager with the given parameters.
        /// </summary>
        public bool Setup(Transform parentTransform, PlayerFarmReference farmReference,
            IProductionRecordReference<RecordProduction> recordProduction, Level level,
            ISaveAbleReference saveAbleReference)
        {
            _farmerSpawner = farmerSpawnerGetGameObject.Get().GetComponent<FarmerSpawner>();
            _isLoaded = true;
            _parent = parentTransform;
            playerFarmReference = farmReference;

            if (_indicatorProgressCapacity == null)
            {
                _indicatorProgressCapacity =
                    indicatorProgressCapacityPrefab.gameObject.Request<IndicatorProgressCapacity>(
                        _parent.position + Vector3.up * height, UnityEngine.Quaternion.Euler(0, 0, 0),
                        _parent
                    );
            }

            bool canStart = Setup(recordProduction.ProductionRecord, level, saveAbleReference);
            if (!canStart) ShowZeroIndicatorCapacity();
            return canStart;
        }


        public void Add(Seed seed)
        {
            RewardClaim();
            queueItem = seed;
            ProductionItemValuePair = new Pair<Item, int>(seed, Required.weightCapacity);
            _indicatorProgressCapacity.Change(seed);
            playerFarmReference.workerPreview.Value = Required.workerCount;
        }

        /// <summary>
        /// Tries to start the progression, overriding the base method to check the 'isClaimable' flag.
        /// </summary>
        public override bool TryStartProgression()
        {
            var canStart = !recordReference.InProgression && !CanClaim && base.TryStartProgression();
            if (canStart)
            {
                ShowIndicatorCapacity();
            }
            else ShowZeroIndicatorCapacity();

            return canStart;
        }

        /// <summary>
        /// Checks if there are enough resources to start the progression.
        /// </summary>
        public override bool HasEnough() =>
            playerFarmReference.inventory.HasEnough(ProductionItemValuePair, WeightCapacity);

        /// <summary>
        /// Checks if the reward can be claimed.
        /// </summary>
        public bool CanClaim
        {
            get => canClaim;
            set => canClaim = value;
        }

        public bool IsLoaded => _isLoaded;


        /// <summary>
        /// Claims the reward if it can be claimed.
        /// </summary>
        [Button]
        public void RewardClaim()
        {
            if (!CanClaim) return;
            _instantiatedRewardPopup.ReturnToPool();
            AddReward().Forget();
        }


        // Private Methods

        /// <summary>
        /// Modifies the record data before the progression starts.
        /// </summary>
        protected override void ModifyRecordBeforeProgression()
        {
            var requiredWorker = Required.workerCount;
            _farmerSpawner.GoToWork(requiredWorker);
            recordReference.worker = new Pair<WorkerType, int>(basicWorkerType, requiredWorker);
            playerFarmReference.workerInventory.TryDecrease(basicWorkerType, requiredWorker);
            ProductionItemValuePair = new Pair<Item, int>(queueItem, WeightCapacity);
            recordReference.Time.Discount = new UnityTimeSpan();
            base.ModifyRecordBeforeProgression();
        }

        /// <summary>
        /// Called when the progression timer starts.
        /// </summary>
        public override void OnTimerStart(float progressRatio)
        {
            meshPlantPointGridSystem.Setup(
                LevelReference, Reward.Key, progressRatio, (float)FullTimeRequirement.TotalSeconds
            );
            ShowIndicatorCapacity();
            if(Mathf.Approximately(progressRatio, 1)) _farmerSpawner.SpawnFarmers(Required.workerCount);
        }

        /// <summary>
        /// Called when the progression timer completes.
        /// </summary>
        public override void OnComplete()
        {
            _indicatorProgressCapacity.gameObject.SetActive(false);
            if (onCompleteParticleEffect) onCompleteParticleEffect.Value.Load(true, _parent).Forget();
            CanClaim = true;
            _instantiatedRewardPopup = popupClickable.gameObject.Request(_parent).GetComponent<PopupClickableIconCount>();
            _instantiatedRewardPopup.Setup(this, this);
            meshPlantPointGridSystem.Complete();
        }

        /// <summary>
        /// Takes the required resources for the progression.
        /// </summary>
        protected override void TakeRequirement()
        {
            var seed = ProductionItemValuePair;
            playerFarmReference.inventory.TryDecrease(seed, ProductionItemValuePair.Value);
        }

        /// <summary>
        /// Adds the reward to the player's inventory.
        /// </summary>
        private async UniTaskVoid AddReward()
        {
            await meshPlantPointGridSystem.ClearAsync();
            ShowZeroIndicatorCapacity();
            ModifyRecordAfterProgression();
            onCompleteParticleEffect.Value.Stop();
            CanClaim = false;
        }

        private void ShowIndicatorCapacity()
        {
            _indicatorProgressCapacity.gameObject.SetActive(true);
            _indicatorProgressCapacity.Setup(ProgressRatio, (float)TimeRemaining.TotalSeconds, false,
                ProductionItemValuePair.Value,
                WeightCapacity, ProductionItemValuePair.Key);
        }

        private void ShowZeroIndicatorCapacity()
        {
            _indicatorProgressCapacity.gameObject.SetActive(true);
            _indicatorProgressCapacity.Change(0, WeightCapacity);
            if (recordReference.InProgression) _indicatorProgressCapacity.Change(ProductionItemValuePair.Key);
            else _indicatorProgressCapacity.ShowDefaultSprite();
        }

        /// <summary>
        /// Modifies the record data after the progression completes.
        /// </summary>
        private void ModifyRecordAfterProgression()
        {
            var convertInfo = ConvertInfo;
            var reward = GetReward(convertInfo);
            var takenWorker = recordReference.worker;
            playerFarmReference.workerInventory.AddOrIncrease(takenWorker);
            _farmerSpawner.RemoveFarmers();
            playerFarmReference.inventory.AddOrIncrease(reward.Key, reward.Value);
            playerFarmReference.levelXp.AddXp((int)XpTotal);
            playerFarmReference.coins.Value += 10;
            recordReference.InProgression = false;
            SaveAbleReference.Save();
        }

        public override string ToString()
        {
            return $"{_parent.name}: {ProductionItemValuePair.Key} x {ProductionItemValuePair.Value}";
        }
    }
}