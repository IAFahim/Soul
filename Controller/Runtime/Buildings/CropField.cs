using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Common;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.Buildings.Managers;
using Soul.Controller.Runtime.Buildings.Records;
using Soul.Controller.Runtime.InfoPanels;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Buildings;
using Soul.Model.Runtime.CustomList;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.SaveAndLoad;
using Soul.Model.Runtime.Selectors;
using Soul.Model.Runtime.Unlocks;
using Soul.Model.Runtime.Upgrades;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Buildings
{
    public class CropField : GameComponent, ISelectCallBack, IGuid, ITitle, ISaveAble, ILocked, ILoadComponent,
        IDropAble<Item>, IUpgrade, IUnlock, ISaveAbleReference, IInfoPanelReference
    {
        [SerializeField, Guid] private string guid;
        [SerializeField] private PlayerInventoryReference playerInventoryReference;
        [SerializeField] private Level level;
        [SerializeField] private CropFieldRecord cropFieldRecord;
        [SerializeField] private CropProductionManager cropProductionManager;
        public AddressablePoolLifetime addressablePoolLifetime;

        [FormerlySerializedAs("lockedInfrastructureInfo")] [SerializeField]
        private LevelInfrastructureInfo levelInfrastructureInfo;

        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private UnlockAndUpgradeManager unlockAndUpgradeManager;

        [SerializeField] private InfoPanel infoPanelPrefab;
        private bool _loadDataOnEnable = true;

        [SerializeField] private ScriptableList<Item> allowedThingsToDrop;

        public string Guid
        {
            get => guid;
            set => guid = value;
        }


        public string Title => levelInfrastructureInfo.Title;
        public bool IsLocked => level.IsLocked;

        public bool MultipleDropMode => false;
        public bool CanDropNow => !IsLocked;

        public bool DropHovering(Item[] thingToDrop)
        {
            foreach (var item in thingToDrop)
            {
                if (!allowedThingsToDrop.Contains(item))
                {
                    return false;
                }
            }

            cropProductionManager.TempAdd(thingToDrop);
            return true;
        }

        public bool TryDrop(Item[] thingToDrop)
        {
            if (DropHovering(thingToDrop))
            {
                if (cropProductionManager.StartProduction())
                {
                    Save(Guid);
                }

                return true;
            }

            return false;
        }

        public ScriptableList<Item> AllowedThingsToDrop => allowedThingsToDrop;

        public void OnSelected(RaycastHit selfRayCastHit) => Debug.Log("Selected: " + this);

        private async void Start()
        {
            if (_loadDataOnEnable) Load(Guid);
            await SetUp(level);
        }


        public async UniTask SetUp(int currentLevel)
        {
            await unlockAndUpgradeManager.Setup(addressablePoolLifetime, playerInventoryReference,
                cropFieldRecord.recordUpgrade, this, boxCollider, level);
            var x = cropProductionManager.Setup(playerInventoryReference, cropFieldRecord.recordProduction, level,
                this);
        }

        public bool CanUpgrade => !level.IsMax;

        [Button]
        public void Upgrade() => unlockAndUpgradeManager.Upgrade(level + 1);

        public override string ToString() => Title;

        [Button]
        public void Save() => Save(Guid);

        [Button]
        public void Load(string key)
        {
            cropFieldRecord = Data.Load(key, cropFieldRecord);
            level.vector2Int = new Vector2Int(cropFieldRecord.level, level.Max);
        }

        public void Save(string key)
        {
            cropFieldRecord.level = level.Current;
            Data.Save(key, cropFieldRecord);
        }

        public Level Level => level;
        public bool IsUpgrading => unlockAndUpgradeManager.IsUpgrading;


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

        public bool IsUnlocking => unlockAndUpgradeManager.IsUnlocking;
        public bool CanUnlock => unlockAndUpgradeManager.HasEnough();

        public void Unlock()
        {
            unlockAndUpgradeManager.Unlock();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(InfoPanelWorldPosition, 1f);
        }

        public IInfoPanel InfoPanelPrefab => infoPanelPrefab;
        public Vector3 InfoPanelWorldPosition => transform.TransformPoint(levelInfrastructureInfo.GetInfoPanelPositionOffset(level));
    }
}