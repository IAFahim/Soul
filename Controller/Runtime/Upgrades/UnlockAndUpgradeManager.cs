using System;
using System.Threading.Tasks;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Requirements;
using Soul.Model.Runtime.SaveAndLoad;
using TrackTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Upgrades
{
    public class UnlockAndUpgradeManager : GameComponent, ILoadComponent
    {
        public PlayerInventoryReference playerInventoryReference;

        [SerializeField]
        private RequirementOfWorkerGroupTimeCurrencyForLevels requirementOfWorkerGroupTimeCurrencyForLevels;

        [FormerlySerializedAs("recordUpgrade")] [SerializeField]
        private RecordUpgrade upgradeRecord;

        public BoxCollider currentBoxCollider;

        [SerializeField] private Transform parent;
        [SerializeField] private Level currentLevel;
        [SerializeField] private bool usePooling;
        [SerializeField] private UnlockManager unlockManager;
        [SerializeField] private Optional<PartsManager> upgradePartsManager;

        private DelayHandle _delayHandle;
        private ISaveAbleReference _saveAbleReference;

        public WorkerGroupTimeCurrencyRequirement<Item, int> Required =>
            requirementOfWorkerGroupTimeCurrencyForLevels.GetRequirement(currentLevel);

        public UnityTimeSpan RequiredFullTime => Required.fullTime;
        public UnityTimeSpan DiscountedTime => upgradeRecord.DiscountedTime(Required.fullTime);
        public UnityTimeSpan TimeRemaining => upgradeRecord.TimeRemaining(Required.fullTime);

        public bool IsUpgrading
        {
            get => upgradeRecord.isUpgrading;
            private set => upgradeRecord.isUpgrading = value;
        }

        public bool IsComplete => IsUpgrading && upgradeRecord.IsCompleted(RequiredFullTime);
        public bool IsUnlocking => currentLevel == 0 && IsUpgrading;


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
                if (upgradePartsManager) upgradePartsManager.Value.Spawn(currentLevel - 1, usePooling, boxCollider);
            }

            if (IsUnlocking) StartTimer(false);
            else if (IsUpgrading) StartTimer(false);
            return IsUpgrading;
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
                OnCompleteUpgrading();
                return;
            }

            float delay = startNow ? (float)DiscountedTime.TotalSeconds : (float)TimeRemaining.TotalSeconds;
            _delayHandle = App.Delay(delay, OnCompleteUpgrading);
            Track.Start(name, delay);
        }

        [Button]
        public void Upgrade(int level)
        {
            UpgradeRecordModify();
            StartTimer(true);
            _saveAbleReference.Save();
        }

        public bool HasEnough()
        {
            return playerInventoryReference.inventory.HasEnough(Required.currency.Key, Required.currency.Value);
        }

        public void UpgradeRecordModify()
        {
            IsUpgrading = true;
            upgradeRecord.workerCount.Value = Required.workerCount;
            upgradeRecord.toLevel = currentLevel + 1;
            upgradeRecord.startTime = new UnityDateTime(DateTime.UtcNow);
            upgradeRecord.reductionTime = new UnityTimeSpan();
        }

        private void OnCompleteUpgrading()
        {
            IsUpgrading = false;
            currentLevel.CurrentLevel = upgradeRecord.toLevel;
            upgradeRecord.isUpgrading = false;
            upgradePartsManager.Value.ClearInstantiatedParts();
            upgradePartsManager.Value.Spawn(currentLevel-1, usePooling);
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
            unlockManager.ReleaseInstance();
            upgradeRecord.toLevel = 1;
            IsUpgrading = true;
            currentLevel.CurrentLevel = 1;
            _saveAbleReference.Save();
        }

        public async UniTask OnCompleteUnlocking()
        {
            await ShowUnlocked(currentBoxCollider);
            OnCompleteUpgrading();
            upgradePartsManager.Value.Spawn(currentLevel - 1, usePooling);
        }
    }
}