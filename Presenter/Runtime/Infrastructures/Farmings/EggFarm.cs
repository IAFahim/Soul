using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake.Common;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Productions;
using UnityEngine;

namespace Soul.Presenter.Runtime.Infrastructures.Farmings
{
    public class EggFarm : FarmingComponent, IProductionRecordReference<RecordProduction>, ILoadComponent
    {
        [Title("EggFarm")] [SerializeField]
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

        public override void ShowUpgradeUnlockPreView(RectTransform parent)
        {
            throw new System.NotImplementedException();
        }

        public override void HideUpgradeUnlockPreView()
        {
            throw new System.NotImplementedException();
        }


        public RecordProduction ProductionRecord { get; set; }

        void ILoadComponent.OnLoadComponents()
        {
            base.Reset();
        }
    }
}