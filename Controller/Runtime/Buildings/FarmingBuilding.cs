using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.InfoPanels;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Buildings;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Selectors;
using Soul.Model.Runtime.Tweens;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class FarmingBuilding : UnlockUpgradeAbleBuilding, IReSelectedCallBack, IInfoPanelReference,
        IUpgradeRecordReference<RecordUpgrade>
    {
        [SerializeField] protected PlayerInventoryReference playerInventoryReference;
        [SerializeField] protected AddressablePoolLifetime addressablePoolLifetimeReference;

        [SerializeField] protected LevelInfrastructureInfo levelInfrastructureInfo;
        [SerializeField] protected BoxCollider boxCollider;
        [SerializeField] protected UnlockAndUpgradeManager unlockAndUpgradeManager;
        [SerializeField] protected InfoPanel infoPanelPrefab;

        private readonly bool _loadDataOnEnable = true;

        #region Title

        public override string Title => levelInfrastructureInfo.Title;

        #endregion


        public virtual async UniTask Setup(AddressablePoolLifetime addressablePoolLifetime,
            PlayerInventoryReference playerInventory)
        {
            addressablePoolLifetimeReference = addressablePoolLifetime;
            playerInventoryReference = playerInventory;
            await SetUp(level);
        }


        private async void Start()
        {
            if (_loadDataOnEnable) Load(Guid);
            await SetUp(level);
        }

        protected virtual async UniTask SetUp(Level currentLevel)
        {
            await unlockAndUpgradeManager.Setup(addressablePoolLifetimeReference, playerInventoryReference,
                this, this, boxCollider, currentLevel);
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
            level.currentAndMax.Set(CurrentLevel, level.Max);
        }

        #region IUpgrade

        public override bool CanUpgrade => !UpgradeRecord.InProgression && !level.IsMax &&
                                           unlockAndUpgradeManager.HasEnough();

        public override bool IsUpgrading => UpgradeRecord.InProgression;

        [Button]
        public override void Upgrade() => unlockAndUpgradeManager.Upgrade(level + 1);

        #endregion


        #region IUnlock

        public override bool CanUnlock => IsLocked && unlockAndUpgradeManager.HasEnough();

        public override bool IsUnlocking => UpgradeRecord.InProgression;

        public override void Unlock()
        {
            unlockAndUpgradeManager.Unlock();
        }

        #endregion

        #region IInfoPanelReference

        public IInfoPanel InfoPanelPrefab => infoPanelPrefab;

        public Vector3 InfoPanelWorldPosition =>
            transform.TransformPoint(levelInfrastructureInfo.GetInfoPanelPositionOffset(level));

        #endregion

        public override string ToString() => Title;

        #region Selected Animation
        
        [SerializeField] protected TweenSettingCurveScriptableObject<Vector3> selectTweenSetting;
        protected MotionHandle SelectTweenMotionHandle;

        protected void PlayDualSquishAndStretch()
        {
            if (SelectTweenMotionHandle.IsActive()) SelectTweenMotionHandle.Cancel();
            SelectTweenMotionHandle = unlockAndUpgradeManager.Transform.TweenPlayer(selectTweenSetting);
        }

        public override void OnSelected(RaycastHit selfRayCastHit)
        {
            PlayDualSquishAndStretch();
        }

        public virtual void OnReSelected(RaycastHit selfReRaycastHit)
        {
            PlayDualSquishAndStretch();
        }

        #endregion

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(InfoPanelWorldPosition, 1f);
        }
    }
}