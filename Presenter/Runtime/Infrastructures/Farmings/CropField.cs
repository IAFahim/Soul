using System.Collections.Generic;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using Pancake.Common;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.Lists;
using Soul.Controller.Runtime.MeshPlanters;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.DragAndDrops;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Productions;
using Soul.Model.Runtime.Tweens;
using Soul.Model.Runtime.Tweens.Scriptable;
using UnityEngine;

namespace Soul.Presenter.Runtime.Infrastructures.Farmings
{
    public class CropField : FarmingComponent, IProductionRecordReference<RecordProduction>,
        ILoadComponent,
        IAllowedToDropReference<Item>, IDropAble<Item>
    {
        [Title("CropField")] [SerializeField] private AllowedItemLists allowedItemLists;
        [SerializeField] private TweenSettingCurveScriptableObject<Vector3> dropTweenSettings;
        [SerializeField] private BuildingAndProductionRecord buildingAndProductionRecord;

        [SerializeField] private CropProductionManager cropProductionManager;

        private MotionHandle _dropMotionHandle;
        private readonly bool _loadDataOnEnable = true;

        #region Title

        public override string Title => infrastructureInfo.Title;

        #endregion

        #region ICurrentLevelReference

        public override int CurrentLevel
        {
            get => buildingAndProductionRecord.level;
            set => buildingAndProductionRecord.level = value;
        }

        #endregion

        #region IUpgradeRecordReference

        public override RecordUpgrade UpgradeRecord
        {
            get => buildingAndProductionRecord.recordUpgrade;
            set => buildingAndProductionRecord.recordUpgrade = value;
        }

        #endregion

        #region IProductionRecordReference

        public RecordProduction ProductionRecord
        {
            get => buildingAndProductionRecord.recordProduction;
            set => buildingAndProductionRecord.recordProduction = value;
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
            if (currentLevel > 0) SetupProduction(currentLevel);
        }

        private void SetupProduction(Level currentLevel)
        {
            cropProductionManager.Setup(
                unlockAndUpgrade.unlockManagerComponent.transform, playerFarm, this, currentLevel, this
            );
        }

        #region ISaveAble

        public override void Save(string key)
        {
            base.Save(key);
            Data.Save(key, buildingAndProductionRecord);
        }

        #endregion

        #region ISaveAbleReference

        [Button]
        public override void Save() => Save(Guid);

        #endregion

        [Button]
        public override void Load(string key)
        {
            buildingAndProductionRecord = Data.Load(key, buildingAndProductionRecord);
            base.Load(key);
        }

        public override void OnUnlockUpgradeStart()
        {
        }

        public override void OnUnlockUpgradeComplete(int _)
        {
            if (!cropProductionManager.IsLoaded) SetupProduction(level);
        }

        public override void ShowUpgradeUnlockPreView(RectTransform parent)
        {
            
        }

        public override void HideUpgradeUnlockPreView()
        {
            
        }


        #region IAllowedToDropReference<Item>

        public IList<Item> ListOfAllowedToDrop => allowedItemLists.CurrentList;

        #endregion

        #region IDropAble<Seed>

        private bool TryDropAdd(Item drop)
        {
            if (!CanDropNow) return false;
            if (drop is Seed seed) cropProductionManager.Add(seed);
            playerFarm.xpPreview.Value = cropProductionManager.XpTotal;
            return ListOfAllowedToDrop.Contains(drop);
        }

        private void DropCleanUp()
        {
            if (_dropMotionHandle.IsActive()) _dropMotionHandle.Complete();
            unlockAndUpgrade.unlockManagerComponent.transform.localScale = dropTweenSettings.start;
            playerFarm.xpPreview.Value = 0;
        }

        public bool CanDropNow => !IsLocked && !IsUpgrading && !ProductionRecord.InProgression;

        public bool OnDrag(Item drop)
        {
            if (_dropMotionHandle.IsActive()) _dropMotionHandle.Complete();
            _dropMotionHandle = unlockAndUpgrade.unlockManagerComponent.transform.TweenScale(dropTweenSettings);
            return TryDropAdd(drop);
        }

        public bool OnDrop(Item dropPackage)
        {
            if (TryDropAdd(dropPackage))
            {
                if (cropProductionManager.TryStartProgression()) Save(Guid);
                DropCleanUp();
                return true;
            }

            DropCleanUp();
            return false;
        }

        public void OnDragCancel()
        {
            DropCleanUp();
        }

        #endregion


        protected override void Reset()
        {
            base.Reset();
            cropProductionManager.meshPlantPointGridSystem = GetComponentInChildren<MeshPlantPointGridSystem>();
        }

        public override bool IsBusy => ProductionRecord.InProgression;

        void ILoadComponent.OnLoadComponents()
        {
            Reset();
        }
    }
}