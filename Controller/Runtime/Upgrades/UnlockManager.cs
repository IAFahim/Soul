using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using Soul.Controller.Runtime.Addressables;
using Soul.Model.Runtime.Containers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Controller.Runtime.Upgrades
{
    public class UnlockManager : GameComponent
    {
        [SerializeField] private AddressablePoolLifetime addressablePoolLifetime;
        [SerializeField] private Optional<AssetReferenceGameObject> lockedAssetReferenceGameObject;
        [SerializeField] private AssetReferenceGameObject unLockedAssetReferenceGameObject;
        [ShowInInspector] private Pair<AssetReferenceGameObject, GameObject> _instantiatedAssetPairReference;

        public void Setup(AddressablePoolLifetime poolLifetime)
        {
            addressablePoolLifetime = poolLifetime;
        }

        private async UniTask<GameObject> InstantiateAsync(AssetReferenceGameObject assetReferenceGameObject)
        {
            ReleaseInstance();
            var instantiatedAsset = await addressablePoolLifetime.GetOrInstantiateAsync(assetReferenceGameObject, Transform);
            _instantiatedAssetPairReference =
                new Pair<AssetReferenceGameObject, GameObject>(assetReferenceGameObject, instantiatedAsset);
            return instantiatedAsset;
        }


        public async UniTask<GameObject> InstantiateLockedAsync()
        {
            if (!lockedAssetReferenceGameObject.Enabled) return null;
            return await InstantiateAsync(lockedAssetReferenceGameObject);
        }


        public async UniTask<GameObject> InstantiateUnLockedAsync()
        {
            return await InstantiateAsync(unLockedAssetReferenceGameObject);
        }

        [Button]
        public void ReleaseInstance()
        {
            if (_instantiatedAssetPairReference.Value)
                addressablePoolLifetime.ReturnToPool(_instantiatedAssetPairReference);
        }
    }
}