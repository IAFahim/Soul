using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using Soul.Controller.Runtime.Addressables;
using Soul.Model.Runtime.Containers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Controller.Runtime.Upgrades
{
    public class UnlockManagerComponent : GameComponent
    {
        [SerializeField] private Optional<AssetReferenceGameObject> lockedAssetReferenceGameObject;
        [SerializeField] private AssetReferenceGameObject unLockedPartsAssetReference;
        
        private Pair<AssetReferenceGameObject, GameObject> _instantiatedAssetPairReference;
        private AddressablePoolLifetime _addressablePoolLifetime;
        public void Setup(AddressablePoolLifetime poolLifetime)
        {
            _addressablePoolLifetime = poolLifetime;
        }

        private async UniTask<GameObject> InstantiateAsync(AssetReferenceGameObject assetReferenceGameObject)
        {
            ReleaseInstance();
            var instantiatedAsset = await _addressablePoolLifetime.GetOrInstantiateAsync(assetReferenceGameObject, Transform);
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
            return await InstantiateAsync(unLockedPartsAssetReference);
        }

        [Button]
        public void ReleaseInstance()
        {
            if (_instantiatedAssetPairReference.Value)
                _addressablePoolLifetime.ReturnToPool(_instantiatedAssetPairReference);
        }
    }
}