using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Common;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.Requirements;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.SaveAndLoad;
using Soul.Model.Runtime.Times;
using UnityEngine;

namespace Soul.Controller.Runtime.Upgrades
{
    public class UnlockAndUpgradeManager : GameComponent, ILoadComponent
    {
        [SerializeField] private RecordUpgrade recordUpgrade;
        [SerializeField] private Transform parent;
        [SerializeField] private Level currentLevel;
        [SerializeField] private bool usePooling;
        [SerializeField] private UnlockManager unlockManager;
        [SerializeField] private Optional<PartsManager> upgradePartsManager;

        private DelayHandle _delayHandle;
        private ISaveAbleReference _saveAbleReference;
        public bool IsUpgrading => _delayHandle is { IsDone: false };


        public async UniTask Setup(AddressablePoolLifetime addressablePoolLifetime, RecordUpgrade upgradeRecord,
            ISaveAbleReference saveAbleReference, BoxCollider boxCollider, Level level)
        {
            recordUpgrade = upgradeRecord;
            _saveAbleReference = saveAbleReference;
            unlockManager.Setup(addressablePoolLifetime);
            currentLevel = level;
            if (currentLevel == 0)
            {
                await unlockManager.InstantiateLockedAsync();
            }
            else
            {
                var unLockedGameObject = await unlockManager.InstantiateUnLockedAsync();
                upgradePartsManager = unLockedGameObject.GetComponent<PartsManager>();
                if (upgradePartsManager) upgradePartsManager.Value.Spawn(currentLevel - 1, usePooling, boxCollider);
            }
        }

        public TimeRequirement UpgradeTimeRequirementFor(int level)
        {
            return new TimeRequirement();
        }

        [Button]
        public void Upgrade(int level)
        {
            recordUpgrade.toLevel = level;
            recordUpgrade.isUpgrading = true;
            _delayHandle = UpgradeTimeRequirementFor(level).Start(OnComplete, true);
            _saveAbleReference.Save();
        }

        private void OnComplete()
        {
            currentLevel.CurrentLevel = recordUpgrade.toLevel;
            upgradePartsManager.Value.Spawn(currentLevel - 1, usePooling);
            recordUpgrade.isUpgrading = false;
            _saveAbleReference.Save();
        }


        void ILoadComponent.OnLoadComponents()
        {
            Reset();
        }

        public void Reset()
        {
            unlockManager = GetComponent<UnlockManager>();
        }
    }
}