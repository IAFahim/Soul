using Alchemy.Inspector;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;
using UnityEngine;

namespace Soul.Presenter.Runtime.Infrastructures.Farmings
{
    public class WeightStorageBaseSelectableComponent : FarmingBaseSelectableComponent
    {
        [Title("WeightStorageBuilding")] [SerializeField]
        private BuildingAndProductionRecord buildingAndProductionRecord;

        public override int CurrentLevel
        {
            get => buildingAndProductionRecord.level;
            set => buildingAndProductionRecord.level = value;
        }

        public override void OnUnlockUpgradeStart()
        {
            
        }

        public override void OnUnlockUpgradeComplete(int toLevel)
        {
        }

        public override RecordUpgrade UpgradeRecord
        {
            get => buildingAndProductionRecord.recordUpgrade;
            set => buildingAndProductionRecord.recordUpgrade = value;
        }

        public override bool IsBusy => false;
    }
}