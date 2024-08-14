using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Common;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.Buildings.Managers;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.Buildings;
using Soul.Model.Runtime.CustomList;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Selectors;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings.Productions
{
    public class CropField : GameComponent, ISelectCallBack, IGuid, ITitle, ISaveAble, ILocked, ILoadComponent, ILevel,
        IDropAble<Item>, IUpgrade
    {
        public AddressablePoolLifetime addressablePoolLifetime;
        [SerializeField, Guid] private string guid;
        [SerializeField] private LockedInfrastructureInfo lockedInfrastructureInfo;
        [SerializeField] private Level level;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private CropProductionManager cropProductionManager;
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

        public bool HoverDrop(Item[] thingToDrop)
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

        public bool Drop(Item[] thingToDrop)
        {
            if (HoverDrop(thingToDrop))
            {
                cropProductionManager.StartProduction();
                return true;
            }

            return false;
        }

        public ScriptableList<Item> AllowedThingsToDrop => allowedThingsToDrop;

        public void OnSelected(RaycastHit selfRaycastHit)
        {
            Debug.Log("Selected: " + this);
        }

        private async void Start()
        {
            if (_loadDataOnEnable) Load(Guid);
            await SetUp(level);
        }


        public async UniTask SetUp(int currentLevel)
        {
            await unlockAndUpgradeManager.Setup(addressablePoolLifetime, boxCollider, currentLevel);
        }

        [Button]
        public void Upgrade()
        {
            unlockAndUpgradeManager.Upgrade(level + 1);
        }

        public override string ToString()
        {
            return Title;
        }

        [Button]
        public void Load(string key)
        {
            level = Data.Load<Level>(Guid, level);
        }

        [Button]
        public void Save(string key)
        {
            Data.Save(key, level);
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