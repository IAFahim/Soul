﻿using System.Collections.Generic;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.InfoPanels;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Controller.Runtime.RequiresAndRewards;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Buildings;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Tweens;
using Soul.Model.Runtime.Tweens.Scriptable;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using Soul.Presenter.Runtime.Slots;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Presenter.Runtime.Infrastructures.Farmings
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class FarmingComponent : UnlockUpgradeAbleComponent,IBusy, IInfoPanelReference,
        IUpgradeRecordReference<RecordUpgrade>, IRequirementForUpgradeScriptableReference
    {
        [Title("FarmingBuilding")] [SerializeField]
        protected AddressablePoolLifetime addressablePoolLifetime;

        [FormerlySerializedAs("playerInventory")] [SerializeField] protected PlayerFarmReference playerFarm;

        [FormerlySerializedAs("levelInfrastructureInfo")] [SerializeField]
        protected LevelInfrastructureInfo infrastructureInfo;


        [SerializeField] protected RequirementForUpgrades requirementForUpgrades;

        [SerializeField] protected UnlockAndUpgrade unlockAndUpgrade;


        [SerializeField] protected InfoPanel infoPanelPrefab;

        
        
        private readonly bool _loadDataOnEnable = true;

        #region Title

        public override string Title => infrastructureInfo.Title;

        #endregion

        public RequirementForUpgrades RequirementForUpgrades => requirementForUpgrades;

        private async void Start()
        {
            if (_loadDataOnEnable) Load(Guid);
            await SetUp(level);
        }

        protected virtual async UniTask SetUp(Level currentLevel)
        {
            var info = new UnlockAndUpgradeSetupInfo
            {
                recordOfUpgrade = this,
                saveAbleReference = this,
                level = currentLevel,
                onUnlockUpgradeStart = OnUnlockUpgradeStart,
                onUnlockUpgradeComplete = OnUnlockUpgradeComplete
            };
            await unlockAndUpgrade.Setup(addressablePoolLifetime, requirementForUpgrades, playerFarm, info);
        }


        #region IUpgradeRecordReference

        public abstract RecordUpgrade UpgradeRecord { get; set; }

        #endregion

        #region ISaveAble

        public override void Save(string key)
        {
            CurrentLevel = level.Current;
        }

        #endregion

        #region ISaveAbleReference

        public override void Save() => Save(Guid);

        #endregion

        public override void Load(string key)
        {
            level.SetWithoutNotify(CurrentLevel, level.Max);
        }

        #region IUpgrade

        public override bool CanUpgrade => !IsBusy && !UpgradeRecord.InProgression && !level.IsMax &&
                                           unlockAndUpgrade.HasEnough();

        public override bool IsUpgrading => UpgradeRecord.InProgression;

        [Button]
        public override void Upgrade() => unlockAndUpgrade.Upgrade();

        #endregion

        #region IUnlock

        public override bool CanUnlock => IsLocked && unlockAndUpgrade.HasEnough();

        public override bool IsUnlocking => UpgradeRecord.InProgression;

        public override void Unlock()
        {
            unlockAndUpgrade.Unlock();
        }

        #endregion

        #region IInfoPanelReference

        public IInfoPanel InfoPanelPrefab => infoPanelPrefab;

        public Vector3 InfoPanelWorldPosition =>
            transform.TransformPoint(infrastructureInfo.GetInfoPanelPositionOffset(level));

        #endregion

        public override string ToString() => Title;

        #region Selected Animation

        [SerializeField] protected TweenSettingCurveScriptableObject<Vector3> selectTweenSetting;
        protected MotionHandle SelectTweenMotionHandle;

        protected void PlayDualSquishAndStretch()
        {
            if (SelectTweenMotionHandle.IsActive()) SelectTweenMotionHandle.Cancel();
            SelectTweenMotionHandle = unlockAndUpgrade.unlockManagerComponent.Transform.TweenScale(selectTweenSetting);
        }

        public override void OnSelected(RaycastHit selfRayCastHit)
        {
            PlayDualSquishAndStretch();
        }

        #endregion

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            if (infrastructureInfo) Gizmos.DrawWireSphere(InfoPanelWorldPosition, 1f);
        }

        protected virtual void Reset()
        {
            unlockAndUpgrade.unlockManagerComponent = GetComponentInChildren<UnlockManagerComponent>();
            addressablePoolLifetime = FindObjectOfType<AddressablePoolLifetime>();
            unlockAndUpgrade.unlockManagerComponent = GetComponentInChildren<UnlockManagerComponent>();
        }

        public abstract bool IsBusy { get; }
    }

    public interface IBusy 
    {
        bool IsBusy { get; }
    }
}