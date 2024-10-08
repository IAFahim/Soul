﻿using System;
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
using Soul.Controller.Runtime.Rewards;
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
            }

            if (upgradePartsManager) upgradePartsManager.Spawn(LevelReference - 1, info.boxCollider);

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
            return playerFarm.coins >= Required.coin && playerFarm.gems >= Required.gem &&
                   playerFarm.workerInventory.HasEnough(Required.worker) &&
                   playerFarm.inventory.HasEnough(Required.items);
        }

        protected override void TakeRequirement()
        {
            playerFarm.coins.Value -= Required.coin;
        }

        protected override void ModifyRecordBeforeProgression()
        {
            playerFarm.workerInventory.TryDecrease(Required.worker);
            playerFarm.gems.Value -= Required.gem;
            playerFarm.coins.Value -= Required.coin;
            recordReference.worker = Required.worker;
            playerFarm.inventory.TryDecrease(Required.items);
            recordReference.toLevel = LevelReference + 1;
            base.ModifyRecordBeforeProgression();
        }

        public virtual void ModifyRecordAfterProgression()
        {
            playerFarm.workerInventory.AddOrIncrease(recordReference.worker);
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
            ModifyRecordAfterProgression();
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