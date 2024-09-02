using System;
using System.Threading;
using System.Threading.Tasks;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using QuickEye.Utility;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.Constructions;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Model.Runtime.Progressions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Controller.Runtime.Upgrades
{
    [Serializable]
    public class UnlockAndUpgradeManager : ProgressionManager<RecordUpgrade>
    {
        public UnlockAndUpgradeSetupInfo info;
        [SerializeField] private RequirementForUpgrades requirementForUpgrades;
        [SerializeField] private Transform parent;
        [SerializeField, DisableInEditMode] private PartsManager upgradePartsManager;

        private AddressablePoolLifetime _addressablePoolLifetime;
        private PlayerInventoryReference _playerInventory;

        #region Enclosure

        [SerializeField] private Optional<AssetReferenceGameObject> upgradeEnclosure;
        private GameObject _enclosureGameObject;
        private IConstruction _constructionEnclose;

        #endregion


        public RequirementForUpgrade Required => requirementForUpgrades.GetRequirement(levelReference);

        public override UnityTimeSpan FullTimeRequirement => Required.fullTime;


        public async UniTask<bool> Setup(AddressablePoolLifetime addressablePoolLifetime,
            PlayerInventoryReference inventoryReference,
            UnlockAndUpgradeSetupInfo unlockAndUpgradeSetupInfo)
        {
            _addressablePoolLifetime = addressablePoolLifetime;
            _playerInventory = inventoryReference;
            info = unlockAndUpgradeSetupInfo;
            info.unlockManagerComponent.Setup(_addressablePoolLifetime);
            return await ValidateStart();
        }

        private async Task ShowUnlocked()
        {
            var unLockedGameObject = await info.unlockManagerComponent.InstantiateUnLockedAsync();
            upgradePartsManager = unLockedGameObject.GetComponent<PartsManager>();
        }

        private async UniTask<bool> ValidateStart()
        {
            bool canStart = base.Setup(info.recordOfUpgrade.UpgradeRecord, info.level, info.saveAbleReference);
            if (levelReference == 0)
            {
                await info.unlockManagerComponent.InstantiateLockedAsync();
            }
            else
            {
                await ShowUnlocked();
                if (upgradePartsManager) upgradePartsManager.Spawn(levelReference - 1, info.boxCollider);
            }

            return canStart;
        }


        [Button]
        public void Upgrade()
        {
            if (TryStartProgression())
            {
                ShowEnclosure().Forget();
            }
        }

        public override bool HasEnough()
        {
            var currentCoin = _playerInventory.coins;
            if (currentCoin.Key != Required.currency.Key) return false;
            return currentCoin >= Required.currency.Value;
        }

        protected override void TakeRequirement()
        {
            _playerInventory.coins.Set(_playerInventory.coins - Required.currency.Value);
        }

        protected override void ModifyRecordBeforeProgression()
        {
            base.ModifyRecordBeforeProgression();
            recordReference.worker.Set(Required.workerCount);
            recordReference.toLevel = levelReference + 1;
        }

        public override void OnTimerStart(bool startsNow)
        {
        }

        #region Enclosure

        private async UniTask ShowEnclosure()
        {
            if (upgradeEnclosure.Enabled)
            {
                await SpawnEnclosure();
                await _constructionEnclose.StartConstruction();
            }
        }

        private async UniTask SpawnEnclosure()
        {
            CancellationToken cancellationToken = default;
            _enclosureGameObject = await upgradeEnclosure.Value.InstantiateAsync(parent)
                .ToUniTask(cancellationToken: cancellationToken);
            _constructionEnclose ??= _enclosureGameObject.GetComponent<IConstruction>();
        }

        private async UniTask EndConstruction()
        {
            if (upgradeEnclosure.Enabled)
            {
                if (_constructionEnclose != null) await _constructionEnclose.EndConstruction();
            }
        }

        #endregion

        public override void OnComplete()
        {
            recordReference.InProgression = false;
            EndConstruction().Forget();
            if (levelReference.IsLocked) OnCompleteUnlockingAsync().Forget();
            else OnCompleteUpgrading();
            info.onUnlockUpgradeComplete?.Invoke(levelReference);
        }


        private void OnCompleteUpgrading()
        {
            levelReference.Current = recordReference.toLevel;
            info.saveAbleReference.Save();
            upgradePartsManager.ClearInstantiatedParts();
            upgradePartsManager.Spawn(levelReference - 1, info.boxCollider);
        }


        public void Unlock()
        {
            if (TryStartProgression())
            {
                ShowEnclosure().Forget();
            }
        }

        private async UniTask OnCompleteUnlockingAsync()
        {
            levelReference.Current = 1;
            info.saveAbleReference.Save();
            await ShowUnlocked();
            upgradePartsManager.Spawn(levelReference - 1, info.boxCollider);
        }

        public override string ToString()
        {
            return $"{parent.name} {levelReference} {recordReference}";
        }
    }
}