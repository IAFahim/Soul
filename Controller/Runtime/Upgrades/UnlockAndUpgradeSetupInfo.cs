using System;
using Soul.Controller.Runtime.Addressables;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.SaveAndLoad;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using UnityEngine;

namespace Soul.Controller.Runtime.Upgrades
{
    [Serializable]
    public class UnlockAndUpgradeSetupInfo
    {
        public UnlockManagerComponent unlockManagerComponent;
        public IUpgradeRecordReference<RecordUpgrade> recordOfUpgrade;
        public ISaveAbleReference saveAbleReference;
        public BoxCollider boxCollider;
        public Level level;
        public Action onUnlockUpgradeStart;
        public Action<int> onUnlockUpgradeComplete;
    }
}