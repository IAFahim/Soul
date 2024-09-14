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
    public class UnlockAndUpgrade : ProgressionManager<RecordUpgrade>
    {
        public UnlockManagerComponent unlockManagerComponent;
        public UnlockAndUpgradeSetupInfo info;
        [SerializeField, DisableInEditMode] private PartsManager upgradePartsManager;

        private RequirementForUpgrades _requirementForUpgrades;
        private AddressablePoolLifetime _addressablePoolLifetime;
        private PlayerFarmReference playerFarm;

        #region Enclosure

        [SerializeField] private Optional<AssetReferenceGameObject> upgradeEnclosure;
        private GameObject _enclosureGameObject;
        private IConstruction _constructionEnclose;

        #endregion


        public RequirementForUpgrade Required => _requirementForUpgrades.GetRequirement(LevelReference);

        public override UnityTimeSpan FullTimeRequirement => Required.fullTime;


        public async UniTask<bool> Setup(AddressablePoolLifetime addressablePoolLifetime,
            RequirementForUpgrades requirementForUpgrades, PlayerFarmReference farmReference,
            UnlockAndUpgradeSetupInfo unlockAndUpgradeSetupInfo
        )
        {
            _addressablePoolLifetime = addressablePoolLifetime;
            _requirementForUpgrades = requirementForUpgrades;
            playerFarm = farmReference;
            info = unlockAndUpgradeSetupInfo;
            unlockManagerComponent.Setup(_addressablePoolLifetime);
            return await ValidateStart();
        }

        private async Task ShowUnlocked()
        {
            var unLockedGameObject = await unlockManagerComponent.InstantiateUnLockedAsync();
            upgradePartsManager = unLockedGameObject.GetComponent<PartsManager>();
        }

        private async UniTask<bool> ValidateStart()
        {
            bool canStart = base.Setup(info.recordOfUpgrade.UpgradeRecord, info.level, info.saveAbleReference);
            if (LevelReference == 0)
            {
                await unlockManagerComponent.InstantiateLockedAsync();
            }
            else
            {
                await ShowUnlocked();
                if (upgradePartsManager) upgradePartsManager.Spawn(LevelReference - 1, info.boxCollider);
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
            var currentCoin = playerFarm.coins;
            if (currentCoin.Key != Required.currency.Key) return false;
            return currentCoin >= Required.currency.Value;
        }

        protected override void TakeRequirement()
        {
            playerFarm.coins.Value -= Required.currency.Value;
        }

        protected override void ModifyRecordBeforeProgression()
        {
            base.ModifyRecordBeforeProgression();
            recordReference.worker.Set(Required.workerCount);
            recordReference.toLevel = LevelReference + 1;
        }

        public override void OnTimerStart(float progressRatio)
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
            _enclosureGameObject = await upgradeEnclosure.Value.InstantiateAsync(unlockManagerComponent.transform)
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
            if (LevelReference.IsLocked) OnCompleteUnlockingAsync().Forget();
            else OnCompleteUpgrading();
            info.onUnlockUpgradeComplete?.Invoke(LevelReference);
        }


        private void OnCompleteUpgrading()
        {
            LevelReference.Current = recordReference.toLevel;
            info.saveAbleReference.Save();
            upgradePartsManager.ClearInstantiatedParts();
            upgradePartsManager.Spawn(LevelReference - 1, info.boxCollider);
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
            LevelReference.Current = 1;
            info.saveAbleReference.Save();
            await ShowUnlocked();
            upgradePartsManager.Spawn(LevelReference - 1, info.boxCollider);
        }

        public override string ToString()
        {
            return $"{unlockManagerComponent.transform.parent.name} {LevelReference} {recordReference}";
        }
    }
}