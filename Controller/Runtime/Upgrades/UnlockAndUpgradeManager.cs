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
        public PlayerInventoryReference playerInventoryReference;
        [SerializeField] private RequirementForUpgrades requirementForUpgrades;

        public BoxCollider currentBoxCollider;

        [SerializeField] private Transform parent;
        [SerializeField] private UnlockManager unlockManager;
        [SerializeField] private PartsManager upgradePartsManager;

        private ISaveAbleReference _saveAbleReference;


        #region Enclosure

        [SerializeField] private Optional<AssetReferenceGameObject> upgradeEnclosure;
        private GameObject _enclosureGameObject;
        private IConstruction _constructionEnclose;

        #endregion


        public RequirementForUpgrade Required => requirementForUpgrades.GetRequirement(levelReference);

        public override UnityTimeSpan FullTimeRequirement => Required.fullTime;


        public async UniTask<bool> Setup(AddressablePoolLifetime addressablePoolLifetime,
            PlayerInventoryReference playerInventory, IUpgradeRecordReference<RecordUpgrade> recordOfUpgrade,
            ISaveAbleReference saveAbleReference, BoxCollider boxCollider, Level level)
        {
            unlockManager.Setup(addressablePoolLifetime);
            bool canStart = base.Setup(recordOfUpgrade.UpgradeRecord, level, saveAbleReference);
            currentBoxCollider = boxCollider;
            playerInventoryReference = playerInventory;
            _saveAbleReference = saveAbleReference;

            if (levelReference == 0)
            {
                await unlockManager.InstantiateLockedAsync();
            }
            else
            {
                await ShowUnlocked();
                if (upgradePartsManager) upgradePartsManager.Spawn(levelReference - 1, boxCollider);
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

        public override void OnTimerStart(bool startsNow)
        {
        }


        public override void OnComplete()
        {
            recordReference.InProgression = false;
            EndConstruction().Forget();
            if (levelReference.IsLocked) OnCompleteUnlockingAsync().Forget();
            else OnCompleteUpgrading();
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
            upgradePartsManager.ClearInstantiatedParts();
            upgradePartsManager.Spawn(levelReference - 1, currentBoxCollider);
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
            if (TryStartProgression())
            {
                ShowEnclosure().Forget();
            }
        }

        private async UniTask OnCompleteUnlockingAsync()
        {
            await ShowUnlocked();
            levelReference.Current = 1;
            upgradePartsManager.Spawn(levelReference - 1, currentBoxCollider);
            _saveAbleReference.Save();
        }

        protected override void TakeRequirement()
        {
            playerInventoryReference.coins.Set(playerInventoryReference.coins - Required.currency.Value);
        }
    }
}