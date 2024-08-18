using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Common;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.Buildings.Managers;
using Soul.Controller.Runtime.Buildings.Records;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Buildings;
using Soul.Model.Runtime.CustomList;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.SaveAndLoad;
using Soul.Model.Runtime.Selectors;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings.Productions
{
    public class CropField : GameComponent, ISelectCallBack, IGuid, ITitle, ISaveAble, ILocked, ILoadComponent, ILevel,
        IDropAble<Item>, IUpgrade, ISaveAbleReference
    {
        [SerializeField, Guid] private string guid;
        [SerializeField] private Level level;
        [SerializeField] private CropFieldRecord cropFieldRecord;
        [SerializeField] private CropProductionManager cropProductionManager;

        public AddressablePoolLifetime addressablePoolLifetime;
        [SerializeField] private LockedInfrastructureInfo lockedInfrastructureInfo;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private UnlockAndUpgradeManager unlockAndUpgradeManager;


        private bool _loadDataOnEnable = true;

        [SerializeField] private ScriptableList<Item> allowedThingsToDrop;

        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        public string Title => lockedInfrastructureInfo.Title;
        public bool IsLocked => level.CurrentLevel == 0;

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

        public void OnSelected(RaycastHit selfRaycastHit) => Debug.Log("Selected: " + this);

        private async void Start()
        {
            if (_loadDataOnEnable) Load(Guid);
            await SetUp(level);
        }


        public async UniTask SetUp(int currentLevel)
        {
            await unlockAndUpgradeManager.Setup(addressablePoolLifetime, cropFieldRecord.recordUpgrade
                , this, boxCollider, level);
            var x = cropProductionManager.Setup(cropFieldRecord.recordProduction, level, this);
        }

        public bool CanUpgrade => !level.IsMaxLevel;

        [Button]
        public void Upgrade() => unlockAndUpgradeManager.Upgrade(level + 1);

        public override string ToString() => Title;

        [Button]
        public void Save() => Save(Guid);

        [Button]
        public void Load(string key)
        {
            cropFieldRecord = Data.Load(key, cropFieldRecord);
            level.level = new Vector2Int(cropFieldRecord.level, level.MaxLevel);
        }

        public void Save(string key)
        {
            cropFieldRecord.level = level.CurrentLevel;
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
    }
}