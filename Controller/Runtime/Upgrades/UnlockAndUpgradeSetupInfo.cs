using System;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.SaveAndLoad;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using UnityEngine;

namespace Soul.Controller.Runtime.Upgrades
{
    [Serializable]
    public class UnlockAndUpgradeSetupInfo
    {
        public AddressablePoolLifetime addressablePoolLifetime;
        public PlayerInventoryReference playerInventory;
        public IUpgradeRecordReference<RecordUpgrade> recordOfUpgrade;
        public ISaveAbleReference saveAbleReference;
        public BoxCollider boxCollider;
        public Level level;
        public Action onUnlockUpgradeStart;
        public Action<int> onUnlockUpgradeComplete;
    }
}