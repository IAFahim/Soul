using System.Collections.Generic;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake.Common;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.CustomList;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Productions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Buildings
{
    public class CropField : FarmingBuilding, IProductionRecordReference<RecordProduction>, ILoadComponent,
        IAllowedToDropReference<Item>, IDropAble<Pair<Item, int>>
    {
        [FormerlySerializedAs("cropFieldRecord")] [SerializeField]
        private ProductionBuildingRecord productionBuildingRecord;

        [SerializeField] private CropProductionManager cropProductionManager;
        [SerializeField] private ScriptableList<Item> allowedThingsToDrop;

        private readonly bool _loadDataOnEnable = true;

        #region Title

        public override string Title => levelInfrastructureInfo.Title;

        #endregion


        #region ICurrentLevelReference

        public override int CurrentLevel
        {
            get => productionBuildingRecord.level;
            set => productionBuildingRecord.level = value;
        }

        #endregion

        #region IUpgradeRecordReference

        public override RecordUpgrade UpgradeRecord
        {
            get => productionBuildingRecord.recordUpgrade;
            set => productionBuildingRecord.recordUpgrade = value;
        }

        #endregion

        #region IProductionRecordReference

        public RecordProduction ProductionRecord
        {
            get => productionBuildingRecord.recordProduction;
            set => productionBuildingRecord.recordProduction = value;
        }

        #endregion

        public override void OnSelected(RaycastHit selfRayCastHit) => Debug.Log("Selected: " + this);

        private async void Start()
        {
            if (_loadDataOnEnable) Load(Guid);
            await SetUp(level);
        }

        protected override async UniTask SetUp(Level currentLevel)
        {
            await base.SetUp(currentLevel);
            cropProductionManager.Setup(playerInventoryReference, this, currentLevel, this);
        }

        #region ISaveAble

        public override void Save(string key)
        {
            base.Save(key);
            Data.Save(key, productionBuildingRecord);
        }

        #endregion

        #region ISaveAbleReference

        [Button]
        public override void Save() => Save(Guid);

        #endregion

        [Button]
        public override void Load(string key)
        {
            productionBuildingRecord = Data.Load(key, productionBuildingRecord);
            base.Load(key);
        }


        #region IAllowedToDropReference<Item>

        public IList<Item> ListOfAllowedToDrop => allowedThingsToDrop;

        #endregion

        #region IDropAble<Pair<Item, int>>

        public bool MultipleDropMode => false;
        public bool CanDropNow => !IsLocked && !IsUpgrading;

        public bool DropHovering(Pair<Item, int> thingToDrop)
        {
            if (!CanDropNow) return false;
            var hasEnough = ListOfAllowedToDrop.Contains(thingToDrop.Key);
            if (hasEnough)
            {
                cropProductionManager.TempAdd(thingToDrop);
            }

            return hasEnough;
        }

        public bool TryDrop(Pair<Item, int> dropPackage)
        {
            if (DropHovering(dropPackage))
            {
                if (cropProductionManager.TryStartProgression())
                {
                    Save(Guid);
                }

                return true;
            }

            return false;
        }

        #endregion


        private void Reset()
        {
            unlockAndUpgradeManager = GetComponentInChildren<UnlockAndUpgradeManager>();
            cropProductionManager = GetComponentInChildren<CropProductionManager>();
            boxCollider = GetComponentInChildren<BoxCollider>();
        }

        void ILoadComponent.OnLoadComponents()
        {
            Reset();
        }
    }
}