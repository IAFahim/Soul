using LitMotion;
using Pancake.Common;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Selectors;
using Soul.Model.Runtime.Tweens;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings
{
    public class EggFarm : FarmingBuilding
    {
        [SerializeField] private ProductionBuildingRecord productionBuildingRecord;
        public Vector3 startScale;
        public Vector3 endScale;
        public float duration;
        public AnimationCurve animationCurve;
        public Transform models;

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

        
    }
}