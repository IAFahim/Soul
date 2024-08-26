using System.Collections.Generic;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using Pancake.Common;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.CustomList;
using Soul.Model.Runtime.DragAndDrops;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Productions;
using Soul.Model.Runtime.Tweens;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings
{
    public class CropField : FarmingBuilding, IProductionRecordReference<RecordProduction>, ILoadComponent,
        IAllowedToDropReference<Item>, IDropAble<Item>
    {
        [SerializeField] private ProductionBuildingRecord productionBuildingRecord;

        [SerializeField] private CropProductionManager cropProductionManager;
        [SerializeField] private ScriptableList<Item> allowedThingsToDrop;
        [SerializeField] private TweenSettingsV3Ya dropTweenSettingsV3Ya;
        private MotionHandle _dropMotionHandle;
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

        #region IDropAble<Seed>

        public bool CanDropNow => !IsLocked && !IsUpgrading;

        public bool OnDrag(Item drop)
        {
            if (_dropMotionHandle.IsActive()) _dropMotionHandle.Complete();
            _dropMotionHandle = unlockAndUpgradeManager.transform.TweenSquishAndStretch(dropTweenSettingsV3Ya);
            return TryDropAdd(drop);
        }

        private bool TryDropAdd(Item drop)
        {
            if (!CanDropNow) return false;
            if (drop is Seed seed) cropProductionManager.Add(seed);
            return ListOfAllowedToDrop.Contains(drop);
        }

        public bool OnDrop(Item dropPackage)
        {
            if (_dropMotionHandle.IsActive()) _dropMotionHandle.Complete();
            unlockAndUpgradeManager.transform.localScale = dropTweenSettingsV3Ya.start;
            if (!TryDropAdd(dropPackage)) return false;
            if (cropProductionManager.TryStartProgression()) Save(Guid);
            return true;
        }

        public void OnDragCancel()
        {
            if (_dropMotionHandle.IsActive()) _dropMotionHandle.Complete();
            unlockAndUpgradeManager.transform.localScale = dropTweenSettingsV3Ya.start;
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