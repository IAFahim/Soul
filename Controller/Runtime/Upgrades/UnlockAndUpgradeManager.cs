using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Common;
using Soul.Controller.Runtime.Addressables;
using Soul.Model.Runtime.Times;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Controller.Runtime.Upgrades
{
    public class UnlockAndUpgradeManager : GameComponent, ILoadComponent
    {
        [SerializeField] private Transform parent;
        [SerializeField] private int currentLevel;
        [SerializeField] private bool usePooling;
        [SerializeField] private UnlockManager unlockManager;
        [SerializeField] private Optional<PartsManager> upgradePartsManager;

        private DelayHandle _delayHandle;
        public bool IsUpgrading => _delayHandle != null && !_delayHandle.IsDone;

        public async UniTask Setup(AddressablePoolLifetime addressablePoolLifetime,
            AssetReferenceGameObject lockedAssetReferenceGameObject,
            AssetReferenceGameObject unLockedAssetReferenceGameObject,
            BoxCollider boxCollider, int level, bool usePoolingForLevels, Transform parentOverride)
        {
            parent = parentOverride;
            await Setup(addressablePoolLifetime, lockedAssetReferenceGameObject, unLockedAssetReferenceGameObject,
                boxCollider, level, usePoolingForLevels);
        }

        public async UniTask Setup(AddressablePoolLifetime addressablePoolLifetime,
            AssetReferenceGameObject lockedAssetReferenceGameObject,
            AssetReferenceGameObject unLockedAssetReferenceGameObject,
            BoxCollider boxCollider, int level, bool usePoolingForLevels)

        {
            usePooling = usePoolingForLevels;
            await Setup(addressablePoolLifetime, lockedAssetReferenceGameObject, unLockedAssetReferenceGameObject,
                boxCollider, level);
        }

        public async UniTask Setup(AddressablePoolLifetime addressablePoolLifetime,
            AssetReferenceGameObject lockedAssetReferenceGameObject,
            AssetReferenceGameObject unLockedAssetReferenceGameObject,
            BoxCollider boxCollider, int level)
        {
            unlockManager.Setup(addressablePoolLifetime, lockedAssetReferenceGameObject,
                unLockedAssetReferenceGameObject);
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
            _delayHandle = UpgradeTimeRequirementFor(level).Start(OnComplete, true);
        }

        private void OnComplete()
        {
            currentLevel++;
            upgradePartsManager.Value.Spawn(currentLevel, usePooling);
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