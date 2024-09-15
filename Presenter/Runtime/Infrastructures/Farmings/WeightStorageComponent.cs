using Alchemy.Inspector;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;
using UnityEngine;

namespace Soul.Presenter.Runtime.Infrastructures.Farmings
{
    public class WeightStorageComponent : FarmingComponent
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

        public override void ShowUpgradeUnlockPreView(RectTransform parent)
        {
            throw new System.NotImplementedException();
        }

        public override void HideUpgradeUnlockPreView()
        {
            throw new System.NotImplementedException();
        }

        public override RecordUpgrade UpgradeRecord
        {
            get => buildingAndProductionRecord.recordUpgrade;
            set => buildingAndProductionRecord.recordUpgrade = value;
        }

        public override bool IsBusy => false;
    }
}