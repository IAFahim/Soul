﻿using System.Collections.Generic;
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
using Soul.Model.Runtime.Containers;
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

namespace Soul.Controller.Runtime.Buildings
{
    public class CropField : GameComponent, ISelectCallBack, IGuid, ITitle, ISaveAble, ILocked, ILoadComponent,
        IDropAble<Pair<Item, int>>, IAllowedToDropReference<Item>, IUpgrade, IUnlock, ISaveAbleReference,
        IInfoPanelReference
    {
        [SerializeField, Guid] private string guid;
        [SerializeField] private PlayerInventoryReference playerInventoryReference;
        [SerializeField] private Level level;
        [SerializeField] private CropFieldRecord cropFieldRecord;
        [SerializeField] private CropProductionManager cropProductionManager;
        public AddressablePoolLifetime addressablePoolLifetime;

        [SerializeField] private LevelInfrastructureInfo levelInfrastructureInfo;

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
        
        public Level Level => level;

        public string Title => levelInfrastructureInfo.Title;
        public bool IsLocked => level.IsLocked;
        public bool IsUpgrading => cropFieldRecord.recordUpgrade.InProgression;
        public bool MultipleDropMode => false;
        public bool CanDropNow => !IsLocked;
        public IInfoPanel InfoPanelPrefab => infoPanelPrefab;

        public Vector3 InfoPanelWorldPosition =>
            transform.TransformPoint(levelInfrastructureInfo.GetInfoPanelPositionOffset(level));

        public IList<Item> ListOfAllowedToDrop => allowedThingsToDrop;

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

        

        [Button]
        public void Upgrade() => unlockAndUpgradeManager.Upgrade(level + 1);

        public bool IsUnlocking => cropFieldRecord.recordUpgrade.InProgression;
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


        public bool DropHovering(Pair<Item, int> thingToDrop)
        {
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