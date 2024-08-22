using System;
using System.Threading.Tasks;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Records;
using Soul.Controller.Runtime.Requirements;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Requirements;
using Soul.Model.Runtime.SaveAndLoad;
using TrackTime;
using UnityEngine;

namespace Soul.Controller.Runtime.Upgrades
{
    public class UnlockAndUpgradeManager : GameComponent, ILoadComponent
    {
        public PlayerInventoryReference playerInventoryReference;

        [SerializeField] private BasicRequirementScriptableObject basicRequirementScriptableObject;

        [SerializeField] private RecordUpgrade upgradeRecord;

        public BoxCollider currentBoxCollider;

        [SerializeField] private Transform parent;
        [SerializeField] private Level currentLevel;
        [SerializeField] private UnlockManager unlockManager;
        [SerializeField] private Optional<PartsManager> upgradePartsManager;

        private DelayHandle _delayHandle;
        private ISaveAbleReference _saveAbleReference;

        public RequirementBasic<Item, int> Required =>
            basicRequirementScriptableObject.GetRequirement(currentLevel);

        public UnityTimeSpan RequiredFullTime => Required.fullTime;
        public UnityTimeSpan GetTimeAfterDiscount => upgradeRecord.Time.GetTimeAfterDiscount(Required.fullTime);
        public UnityTimeSpan TimeRemaining => upgradeRecord.Time.Remaining(Required.fullTime);


        public bool IsComplete => upgradeRecord.InProgression && upgradeRecord.Time.IsOver(RequiredFullTime);
        public bool IsUnlocking => currentLevel == 0 && upgradeRecord.InProgression;


        public async UniTask<bool> Setup(AddressablePoolLifetime addressablePoolLifetime,
            PlayerInventoryReference playerInventory, RecordUpgrade recordOfUpgrade,
            ISaveAbleReference saveAbleReference, BoxCollider boxCollider, Level level)
        {
            upgradeRecord = recordOfUpgrade;
            currentBoxCollider = boxCollider;
            playerInventoryReference = playerInventory;
            _saveAbleReference = saveAbleReference;
            unlockManager.Setup(addressablePoolLifetime);
            currentLevel = level;
            if (currentLevel == 0)
            {
                await unlockManager.InstantiateLockedAsync();
            }
            else
            {
                await ShowUnlocked(boxCollider);
                if (upgradePartsManager) upgradePartsManager.Value.Spawn(currentLevel - 1, boxCollider);
            }

            if (IsUnlocking) StartTimer(false);
            return upgradeRecord.InProgression;
        }

        private async Task ShowUnlocked(BoxCollider boxCollider)
        {
            var unLockedGameObject = await unlockManager.InstantiateUnLockedAsync();
            upgradePartsManager = unLockedGameObject.GetComponent<PartsManager>();
        }

        private void StartTimer(bool startNow)
        {
            if (IsComplete)
            {
                if (currentLevel.IsLocked) OnCompleteUnlocking();
                else OnCompleteUpgrading();
                return;
            }

            float delay = startNow ? (float)GetTimeAfterDiscount.TotalSeconds : (float)TimeRemaining.TotalSeconds;
            _delayHandle = App.Delay(delay, currentLevel.IsLocked ? OnCompleteUnlocking : OnCompleteUpgrading);
            Track.Start(name, delay);
        }

        [Button]
        public void Upgrade(int level)
        {
            RecordModify();
            StartTimer(true);
            _saveAbleReference.Save();
        }

        public bool HasEnough()
        {
            return playerInventoryReference.inventory.HasEnough(Required.currency.Key, Required.currency.Value);
        }

        private void RecordModify()
        {
            upgradeRecord.InProgression = true;
            upgradeRecord.SetWorker(Required.workerCount);
            upgradeRecord.toLevel = currentLevel + 1;
            upgradeRecord.Time.StartedAt = new UnityDateTime(DateTime.UtcNow);
            upgradeRecord.Time.Discount = new UnityTimeSpan();
        }

        private void OnCompleteUpgrading()
        {
            upgradeRecord.InProgression = false;
            currentLevel.Current = upgradeRecord.toLevel;
            upgradePartsManager.Value.ClearInstantiatedParts();
            upgradePartsManager.Value.Spawn(currentLevel - 1, currentBoxCollider);
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

        public void Unlock()
        {
            RecordModify();
            StartTimer(true);
            _saveAbleReference.Save();
        }

        private void OnCompleteUnlocking()
        {
            OnCompleteUnlockingAsync().Forget();
        }

        private async UniTask OnCompleteUnlockingAsync()
        {
            upgradeRecord.InProgression = false;
            await ShowUnlocked(currentBoxCollider);
            currentLevel.Current = 1;
            upgradePartsManager.Value.Spawn(currentLevel - 1, currentBoxCollider);
            _saveAbleReference.Save();
        }
    }
}