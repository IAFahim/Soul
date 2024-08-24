using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.InfoPanels;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Buildings;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings
{
    public abstract class GameBuilding : UnlockUpgradeAbleBuilding, IInfoPanelReference,
        IUpgradeRecordReference<RecordUpgrade>
    {
        [SerializeField] protected PlayerInventoryReference playerInventoryReference;
        [SerializeField] protected AddressablePoolLifetime addressablePoolLifetime;
        [SerializeField] protected LevelInfrastructureInfo levelInfrastructureInfo;
        [SerializeField] protected BoxCollider boxCollider;
        [SerializeField] protected UnlockAndUpgradeManager unlockAndUpgradeManager;
        [SerializeField] protected InfoPanel infoPanelPrefab;

        private readonly bool _loadDataOnEnable = true;

        #region Title

        public override string Title => levelInfrastructureInfo.Title;

        #endregion


        private async void Start()
        {
            if (_loadDataOnEnable) Load(Guid);
            await SetUp(level);
        }

        protected virtual async UniTask SetUp(Level currentLevel)
        {
            await unlockAndUpgradeManager.Setup(addressablePoolLifetime, playerInventoryReference,
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

        [Button]
        public override void Save() => Save(Guid);

        #endregion

        [Button]
        public override void Load(string key)
        {
            level.currentAndMax = new Vector2Int(CurrentLevel, level.Max);
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

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(InfoPanelWorldPosition, 1f);
        }
    }
}