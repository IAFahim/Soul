using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using Pancake;
using Soul.Controller.Runtime.AddressablesHelper;
using Soul.Model.Runtime.Containers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Controller.Runtime.Upgrades
{
    public class UnlockManager : GameComponent
    {
        [SerializeField] private AddressablePoolLifetime addressablePoolLifetime;
        [SerializeField] private AssetReferenceGameObject lockedAssetReferenceGameObject;
        [SerializeField] private AssetReferenceGameObject unLockedAssetReferenceGameObject;
        [SerializeField] private int usePooling;
        [ShowInInspector] private Pair<AssetReferenceGameObject, GameObject> _instantiatedAssetPairReference;

        public void Setup(AddressablePoolLifetime poolLifetime,
            AssetReferenceGameObject lockedAssetReference,
            AssetReferenceGameObject unLockedAssetReference)
        {
            lockedAssetReferenceGameObject = lockedAssetReference;
            unLockedAssetReferenceGameObject = unLockedAssetReference;
            addressablePoolLifetime = poolLifetime;
        }

        private async UniTask<GameObject> InstantiateAsync(AssetReferenceGameObject assetReferenceGameObject)
        {
            ReleaseInstance();
            var instantiatedAsset = await addressablePoolLifetime.GetOrInstantiateAsync(
                assetReferenceGameObject, transform.position, Quaternion.identity, Transform, usePooling
            );
            if (usePooling > -1) usePooling++;
            _instantiatedAssetPairReference =
                new Pair<AssetReferenceGameObject, GameObject>(assetReferenceGameObject, instantiatedAsset);
            return instantiatedAsset;
        }


        public async UniTask<GameObject> InstantiateLockedAsync() =>
            await InstantiateAsync(lockedAssetReferenceGameObject);

        public async UniTask<GameObject> InstantiateUnLockedAsync() =>
            await InstantiateAsync(unLockedAssetReferenceGameObject);

        [Button]
        private void ReleaseInstance()
        {
            if (_instantiatedAssetPairReference.Value)
                addressablePoolLifetime.ReturnToPoolOrDestroy(_instantiatedAssetPairReference, usePooling);
        }
    }
}