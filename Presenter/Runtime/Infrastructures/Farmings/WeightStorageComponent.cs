using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.UpgradeAndUnlock;
using Soul.Presenter.Runtime.Slots;
using UnityEngine;

namespace Soul.Presenter.Runtime.Infrastructures.Farmings
{
    public class WeightStorageComponent : FarmingComponent
    {
        [Title("WeightStorageBuilding")] [SerializeField]
        private BuildingAndProductionRecord buildingAndProductionRecord;

        [SerializeField] private int[] weightCapacityLevel = { 300, 500, 1000, 1500, 2500 };

        public override int CurrentLevel
        {
            get => buildingAndProductionRecord.level;
            set => buildingAndProductionRecord.level = value;
        }

        protected override UniTask SetUp(Level currentLevel)
        {
            var setup = base.SetUp(currentLevel);
            playerFarm.weight.Value.Max = weightCapacityLevel[CurrentLevel - 1];
            playerFarm.weight.Value.Current = playerFarm.weight.Value.Current;
            return setup;
        }

        public override void OnUnlockUpgradeStart()
        {
        }

        public override void OnUnlockUpgradeComplete(int toLevel)
        {
        }

        public override void ShowUpgradeUnlockPreview(RectTransform parent)
        {
            float currentLevelWeight = weightCapacityLevel[CurrentLevel - 1];
            float nextLevelWeight = weightCapacityLevel[CurrentLevel];
            float maxWeight = weightCapacityLevel[^1];
            var upgradeSlot = upgradeSlotPrefab.gameObject.Request<UpgradeSlot>(parent);
            upgradeSlot.Setup("Weight Limit", currentLevelWeight, nextLevelWeight, maxWeight);
        }

        public override RecordUpgrade UpgradeRecord
        {
            get => buildingAndProductionRecord.recordUpgrade;
            set => buildingAndProductionRecord.recordUpgrade = value;
        }

        public override bool IsBusy => false;
    }
}