using Cysharp.Threading.Tasks;
using Pancake.Common;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Productions;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings
{
    public class EggFarm : FarmingBuilding, IProductionRecordReference<RecordProduction>
    {
        [SerializeField] private ProductionBuildingRecord productionBuildingRecord;
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
                playerInventoryReference, productionBuildingRecord.recordProduction, currentLevel, this
            );
        }

        public override int CurrentLevel
        {
            get => productionBuildingRecord.level;
            set => productionBuildingRecord.level = value;
        }

        public override RecordUpgrade UpgradeRecord
        {
            get => productionBuildingRecord.recordUpgrade;
            set => productionBuildingRecord.recordUpgrade = value;
        }

        public override void Save(string key)
        {
            base.Save(key);
            Data.Save(key, productionBuildingRecord);
        }

        public override void Load(string key)
        {
            productionBuildingRecord = Data.Load(key, productionBuildingRecord);
            base.Load(key);
        }

        public RecordProduction ProductionRecord { get; set; }
    }
}