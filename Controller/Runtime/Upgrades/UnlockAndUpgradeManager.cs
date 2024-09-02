using System;
using System.Threading;
using System.Threading.Tasks;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Controller.Runtime.Addressables;
using Soul.Controller.Runtime.Constructions;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Requirements;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.SaveAndLoad;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Controller.Runtime.Upgrades
{
    public class UnlockAndUpgradeManager : ProgressionManager<RecordUpgrade>, ILoadComponent
    {
        public UnlockAndUpgradeSetupInfo info;
        [SerializeField] private RequirementForUpgrades requirementForUpgrades;
        [SerializeField] private Transform parent;
        [SerializeField] private UnlockManager unlockManager;
        [SerializeField] private PartsManager upgradePartsManager;
        

        #region Enclosure

        [SerializeField] private Optional<AssetReferenceGameObject> upgradeEnclosure;
        private GameObject _enclosureGameObject;
        private IConstruction _constructionEnclose;

        #endregion


        public RequirementForUpgrade Required => requirementForUpgrades.GetRequirement(levelReference);

        public override UnityTimeSpan FullTimeRequirement => Required.fullTime;


        public async UniTask<bool> Setup(UnlockAndUpgradeSetupInfo unlockAndUpgradeSetupInfo)
        {
            info = unlockAndUpgradeSetupInfo;
            unlockManager.Setup(info.addressablePoolLifetime);
            bool canStart = base.Setup(info.recordOfUpgrade.UpgradeRecord, info.level, info.saveAbleReference);

            if (levelReference == 0)
            {
                await unlockManager.InstantiateLockedAsync();
            }
            else
            {
                await ShowUnlocked();
                if (upgradePartsManager) upgradePartsManager.Spawn(levelReference - 1, info.boxCollider);
            }

            return canStart;
        }


        private async Task ShowUnlocked()
        { 
            var unLockedGameObject = await unlockManager.InstantiateUnLockedAsync();
            upgradePartsManager = unLockedGameObject.GetComponent<PartsManager>();
        }

        [Button]
        public void Upgrade(int level)
        {
            if (TryStartProgression())
            {
                ShowEnclosure().Forget();
            }
        }

        public override bool HasEnough()
        {
            var currentCoin = info.playerInventory.coins;
            if (currentCoin.Key != Required.currency.Key) return false;
            return currentCoin >= Required.currency.Value;
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


        public override void OnComplete()
        {
            recordReference.InProgression = false;
            EndConstruction().Forget();
            if (levelReference.IsLocked) OnCompleteUnlockingAsync().Forget();
            else OnCompleteUpgrading();
            info.onUnlockUpgradeComplete?.Invoke(levelReference);
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

        private void OnCompleteUpgrading()
        {
            levelReference.Current = recordReference.toLevel;
            info.saveAbleReference.Save();
            upgradePartsManager.ClearInstantiatedParts();
            upgradePartsManager.Spawn(levelReference - 1, info.boxCollider);
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

        protected override void TakeRequirement()
        {
            info.playerInventory.coins.Set(info.playerInventory.coins - Required.currency.Value);
        }
    }
}