using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Model.Runtime.AddressableHelper
{
    [Serializable]
    public class AsyncAssetReferenceGameObject
    {
        [SerializeField] private AssetReferenceGameObject assetReference;
        private object _key;
        private bool _isDisposed;

        public void LoadRuntimeKey()
        {
            _key = assetReference.RuntimeKey;
        }

        public async Task<GameObject> InstantiateAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await Addressables.InstantiateAsync(_key).ToUniTask(cancellationToken: cancellationToken);
        }

        public async Task<GameObject> InstantiateAsync(Transform parent, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await Addressables.InstantiateAsync(_key, parent).ToUniTask(cancellationToken: cancellationToken);
        }

        public async Task<GameObject> InstantiateAsync(Vector3 position, Quaternion rotation,
            CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await Addressables.InstantiateAsync(_key, position, rotation)
                .ToUniTask(cancellationToken: cancellationToken);
        }

        public async Task<GameObject> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent,
            CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await Addressables.InstantiateAsync(_key, position, rotation, parent)
                .ToUniTask(cancellationToken: cancellationToken);
        }

        public async Task<GameObject> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent,
            bool instantiateInWorldSpace, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await Addressables.InstantiateAsync(_key, position, rotation, parent, instantiateInWorldSpace)
                .ToUniTask(cancellationToken: cancellationToken);
        }

        public void Dispose()
        {
            ThrowIfDisposed();
            _isDisposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException(GetType().Name);
        }
    }
}