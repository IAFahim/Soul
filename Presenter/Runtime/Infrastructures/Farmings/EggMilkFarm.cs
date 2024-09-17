using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake.Common;
using Pancake.Pools;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Productions;
using Soul.Presenter.Runtime.Slots;
using UnityEngine;

namespace Soul.Presenter.Runtime.Infrastructures.Farmings
{
    public class EggMilkFarm : FarmingComponent, IProductionRecordReference<RecordProduction>, ILoadComponent
    {
        [Title("EggMilkFarm")] [SerializeField]
        private BuildingAndProductionRecord buildingAndProductionRecord;

        [SerializeField] private EggProductionManager eggProductionManager;
        
        private readonly bool _loadDataOnEnable = true;

        public async void Start()
        {
            if (_loadDataOnEnable) Load(Guid);
            await SetUp(level);
        }

        protected override async UniTask SetUp(Level currentLevel)
        {
            await base.SetUp(currentLevel);
            eggProductionManager.Setup(
                transform, playerFarm, buildingAndProductionRecord.recordProduction, currentLevel, this
            );
        }

        public override int CurrentLevel
        {
            get => buildingAndProductionRecord.level;
            set => buildingAndProductionRecord.level = value;
        }

        public override RecordUpgrade UpgradeRecord
        {
            get => buildingAndProductionRecord.recordUpgrade;
            set => buildingAndProductionRecord.recordUpgrade = value;
        }

        public override void Save(string key)
        {
            base.Save(key);
            Data.Save(key, buildingAndProductionRecord);
        }

        public override void Load(string key)
        {
            buildingAndProductionRecord = Data.Load(key, buildingAndProductionRecord);
            base.Load(key);
        }

        public override bool IsBusy => false;

        public override void OnUnlockUpgradeStart()
        {
            eggProductionManager.Cancel();
        }

        public override void OnUnlockUpgradeComplete(int obj)
        {
            eggProductionManager.TryStartProgression();
        }

        public override void ShowUpgradeUnlockPreview(RectTransform parent)
        {
            AddUpgradeSlots(parent);
        }

        private void AddUpgradeSlots(RectTransform parent)
        {
            float currentTimeRate = 0;
            float currentCapacity = 0;
            float currentProductionRate = 0;
            if (!level.IsLocked)
            {
                var requiredCurrent = eggProductionManager.requiredAndRewardForProductions.GetRequirement(level - 1);
                currentTimeRate = requiredCurrent.timeMultiplier;
                currentCapacity = requiredCurrent.weightCapacity;
                var rewardCurrent = eggProductionManager.requiredAndRewardForProductions.GetReward(level - 1);
                currentProductionRate = rewardCurrent.productionMultiplier;
            }

            var maxRequirement = eggProductionManager.requiredAndRewardForProductions.GetMaxRequirement();
            var maxReward = eggProductionManager.requiredAndRewardForProductions.GetMaxReward();
            float maxTimeRate = maxRequirement.timeMultiplier;
            float maxCapacity = maxRequirement.weightCapacity;
            float maxProductionRate = maxReward.productionMultiplier;

            float nextTimeRate;
            float nextProductionRate;
            float nextCapacity;
            if (!level.IsMax)
            {
                var requiredAndRewardNext = eggProductionManager.requiredAndRewardForProductions.GetRequirement(level);
                nextTimeRate = requiredAndRewardNext.timeMultiplier;
                nextCapacity = requiredAndRewardNext.weightCapacity;
                var rewardNext = eggProductionManager.requiredAndRewardForProductions.GetReward(level);
                nextProductionRate = rewardNext.productionMultiplier;
            }
            else
            {
                nextTimeRate = maxTimeRate;
                nextProductionRate = maxProductionRate;
                nextCapacity = maxCapacity;
            }

            AddUpgradeSlot(parent, "Time Rate", currentTimeRate, nextTimeRate, maxTimeRate, true);
            AddUpgradeSlot(parent, "Production Rate", currentProductionRate, nextProductionRate, maxProductionRate,
                true);
            AddUpgradeSlot(parent, "Capacity", currentCapacity, nextCapacity, maxCapacity, false);
        }

        private void AddUpgradeSlot(RectTransform parent, string timeRate, float currentTimeRate, float nextTimeRate,
            float maxTimeRate, bool percentage)
        {
            var upgradeSlot = upgradeSlotPrefab.gameObject.Request<UpgradeSlot>(parent);
            upgradeSlot.Setup(timeRate, currentTimeRate, nextTimeRate, maxTimeRate, percentage);
            upgradeSlots.Add(upgradeSlot);
        }

        


        public RecordProduction ProductionRecord { get; set; }

        void ILoadComponent.OnLoadComponents()
        {
            base.Reset();
        }
    }
}