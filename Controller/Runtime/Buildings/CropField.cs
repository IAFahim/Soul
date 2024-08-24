using System.Collections.Generic;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake.Common;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.Buildings.Managers;
using Soul.Controller.Runtime.Buildings.Records;
using Soul.Controller.Runtime.InfoPanels;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Buildings;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.CustomList;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings
{
    public class CropField : UnlockUpgradeAbleBuilding, ILoadComponent, IAllowedToDropReference<Item>,
        IDropAble<Pair<Item, int>>, IInfoPanelReference
    {
        [SerializeField] private PlayerInventoryReference playerInventoryReference;
        [SerializeField] private CropFieldRecord cropFieldRecord;
        [SerializeField] private CropProductionManager cropProductionManager;
        public AddressablePoolLifetime addressablePoolLifetime;
        [SerializeField] private LevelInfrastructureInfo levelInfrastructureInfo;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private UnlockAndUpgradeManager unlockAndUpgradeManager;
        [SerializeField] private InfoPanel infoPanelPrefab;
        [SerializeField] private ScriptableList<Item> allowedThingsToDrop;

        private bool _loadDataOnEnable = true;

        #region Title

        public override string Title => levelInfrastructureInfo.Title;

        #endregion


        public override void OnSelected(RaycastHit selfRayCastHit) => Debug.Log("Selected: " + this);

        private async void Start()
        {
            if (_loadDataOnEnable) Load(Guid);
            await SetUp(level);
        }

        private async UniTask SetUp(Level currentLevel)
        {
            await unlockAndUpgradeManager.Setup(addressablePoolLifetime, playerInventoryReference,
                cropFieldRecord.recordUpgrade, this, boxCollider, currentLevel);
            cropProductionManager.Setup(playerInventoryReference, cropFieldRecord.recordProduction, currentLevel, this);
        }

        #region ISaveAble

        public override void Save(string key)
        {
            cropFieldRecord.level = level.Current;
            Data.Save(key, cropFieldRecord);
        }

        #endregion

        #region ISaveAbleReference

        [Button]
        public override void Save() => Save(Guid);

        #endregion

        [Button]
        public override void Load(string key)
        {
            cropFieldRecord = Data.Load(key, cropFieldRecord);
            level.vector2Int = new Vector2Int(cropFieldRecord.level, level.Max);
        }

        #region IUpgrade

        public override bool CanUpgrade => !cropFieldRecord.recordProduction.InProgression && !level.IsMax &&
                                           unlockAndUpgradeManager.HasEnough();

        public override bool IsUpgrading => cropFieldRecord.recordUpgrade.InProgression;

        [Button]
        public override void Upgrade() => unlockAndUpgradeManager.Upgrade(level + 1);

        #endregion


        #region IUnlock

        public override bool CanUnlock => IsLocked && unlockAndUpgradeManager.HasEnough();

        public override bool IsUnlocking => cropFieldRecord.recordUpgrade.InProgression;

        public override void Unlock()
        {
            unlockAndUpgradeManager.Unlock();
        }

        #endregion

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


        #region IInfoPanelReference

        public IInfoPanel InfoPanelPrefab => infoPanelPrefab;

        public Vector3 InfoPanelWorldPosition =>
            transform.TransformPoint(levelInfrastructureInfo.GetInfoPanelPositionOffset(level));

        #endregion

        public override string ToString() => Title;

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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(InfoPanelWorldPosition, 1f);
        }
    }
}