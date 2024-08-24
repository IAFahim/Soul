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
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.SaveAndLoad;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using TrackTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Upgrades
{
    public class UnlockAndUpgradeManager : ProgressionManager<RecordUpgrade> , ILoadComponent
    {
        public PlayerInventoryReference playerInventoryReference;

        [FormerlySerializedAs("requirementScriptableObject")]
        [FormerlySerializedAs("basicRequirementScriptableObject")]
        [SerializeField]
        private RequirementForUpgrades requirementForUpgrades;

        public BoxCollider currentBoxCollider;

        [SerializeField] private Transform parent;
        [SerializeField] private UnlockManager unlockManager;
        [SerializeField] private Optional<PartsManager> upgradePartsManager;

        private ISaveAbleReference _saveAbleReference;

        public RequirementForUpgrade Required => requirementForUpgrades.GetRequirement(levelReference);

        public override UnityTimeSpan FullTimeRequirement => Required.fullTime;


        public async UniTask<bool> Setup(AddressablePoolLifetime addressablePoolLifetime,
            PlayerInventoryReference playerInventory, IUpgradeRecordReference<RecordUpgrade> recordOfUpgrade,
            ISaveAbleReference saveAbleReference, BoxCollider boxCollider, Level level)
        {
            bool canStart = base.Setup(recordOfUpgrade.UpgradeRecord, level, saveAbleReference);
            currentBoxCollider = boxCollider;
            playerInventoryReference = playerInventory;
            _saveAbleReference = saveAbleReference;
            unlockManager.Setup(addressablePoolLifetime);
            
            if (levelReference == 0)
            {
                await unlockManager.InstantiateLockedAsync();
            }
            else
            {
                await ShowUnlocked(boxCollider);
                if (upgradePartsManager) upgradePartsManager.Value.Spawn(levelReference - 1, boxCollider);
            }
            
            return canStart;
        }

        private async Task ShowUnlocked(BoxCollider boxCollider)
        {
            var unLockedGameObject = await unlockManager.InstantiateUnLockedAsync();
            upgradePartsManager = unLockedGameObject.GetComponent<PartsManager>();
        }

        [Button]
        public void Upgrade(int level)
        {
            TryStartProgression();
        }

        public override bool HasEnough()
        {
            var currentCoin = playerInventoryReference.coins;
            if (currentCoin.Key != Required.currency.Key) return false;
            return currentCoin >= Required.currency.Value;
        }

        protected override void ModifyRecordBeforeProgression()
        {
            base.ModifyRecordBeforeProgression();
            recordReference.worker.Set(Required.workerCount);
            recordReference.toLevel = levelReference + 1;
        }

        public override void OnTimerStart()
        {
        }

        public override void OnComplete()
        {
            recordReference.InProgression = false;
            if (levelReference.IsLocked) OnCompleteUnlockingAsync().Forget();
            else OnCompleteUpgrading();
        }

        private void OnCompleteUpgrading()
        {
            levelReference.Current = recordReference.toLevel;
            upgradePartsManager.Value.ClearInstantiatedParts();
            upgradePartsManager.Value.Spawn(levelReference - 1, currentBoxCollider);
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
            TryStartProgression();
        }

        private async UniTask OnCompleteUnlockingAsync()
        {
            await ShowUnlocked(currentBoxCollider);
            levelReference.Current = 1;
            upgradePartsManager.Value.Spawn(levelReference - 1, currentBoxCollider);
            _saveAbleReference.Save();
        }

        protected override void TakeRequirement()
        {
            playerInventoryReference.coins.Set(playerInventoryReference.coins - Required.currency.Value);
        }
    }
}