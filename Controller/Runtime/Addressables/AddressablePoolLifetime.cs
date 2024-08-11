using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using Soul.Model.Runtime.Containers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Controller.Runtime.Addressables
{
    public class AddressablePoolLifetime : MonoBehaviour
    {
        private readonly Dictionary<AssetReferenceGameObject, AsyncAddressableGameObjectPool> _pools = new();

        public async UniTask<GameObject> GetOrInstantiateAsync(AssetReferenceGameObject assetReference,
            CancellationToken cancellationToken = default)
        {
            if (!_pools.TryGetValue(assetReference, out var pool))
            {
                pool = new AsyncAddressableGameObjectPool(assetReference);
                _pools[assetReference] = pool;
            }

            return await pool.RequestAsync(cancellationToken);
        }

        public async UniTask<GameObject> GetOrInstantiateAsync(AssetReferenceGameObject assetReference,
            Transform parent, CancellationToken cancellationToken = default)
        {
            if (!_pools.TryGetValue(assetReference, out var pool))
            {
                pool = new AsyncAddressableGameObjectPool(assetReference);
                _pools[assetReference] = pool;
            }

            return await pool.RentAsync(parent, cancellationToken);
        }

        public async UniTask<GameObject> GetOrInstantiateAsync(AssetReferenceGameObject assetReference,
            Vector3 position, Quaternion rotation, CancellationToken cancellationToken = default)
        {
            if (!_pools.TryGetValue(assetReference, out var pool))
            {
                pool = new AsyncAddressableGameObjectPool(assetReference);
                _pools[assetReference] = pool;
            }

            return await pool.RentAsync(position, rotation, cancellationToken);
        }

        public async UniTask<GameObject> GetOrInstantiateAsync(AssetReferenceGameObject assetReference,
            Vector3 position, Quaternion rotation, Transform parent)
        {
            if (!_pools.TryGetValue(assetReference, out var pool))
            {
                pool = new AsyncAddressableGameObjectPool(assetReference);
                _pools[assetReference] = pool;
            }

            return await pool.RentAsync(position, rotation, parent);
        }

        public void ReturnToPool(AssetReferenceGameObject assetReference, GameObject spawnedGameObject)
        {
            if (_pools.TryGetValue(assetReference, out var pool))
            {
                pool.Return(spawnedGameObject);
            }
            else
            {
                Debug.LogWarning($"Trying to return an object to a non-existent pool for asset: {assetReference}");
                Destroy(spawnedGameObject);
            }
        }
        
        public void ReturnToPool(Pair<AssetReferenceGameObject, GameObject>  assetReferenceGameObjectPair)
        {
            ReturnToPool(assetReferenceGameObjectPair.Key, assetReferenceGameObjectPair.Value);
        }

        public async UniTask PrewarmAsync(AssetReferenceGameObject assetReference, int count,
            CancellationToken cancellationToken = default)
        {
            if (!_pools.TryGetValue(assetReference, out var pool))
            {
                pool = new AsyncAddressableGameObjectPool(assetReference);
                _pools[assetReference] = pool;
            }

            await pool.PrewarmAsync(count, cancellationToken);
        }

        private void OnDestroy()
        {
            foreach (var pool in _pools.Values)
            {
                pool.Dispose();
            }

            _pools.Clear();
        }
    }
}