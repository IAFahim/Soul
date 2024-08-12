using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using Soul.Model.Runtime.Containers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Controller.Runtime.AddressablesHelper
{
    public class AddressablePoolLifetime : MonoBehaviour
    {
        private readonly Dictionary<AssetReferenceGameObject, AsyncAddressableGameObjectPool> _pools = new();

        public async UniTask<GameObject> GetOrInstantiateAsync(AssetReferenceGameObject assetReference, int usePool,
            CancellationToken cancellationToken = default)
        {
            if (usePool > -1 && !_pools.TryGetValue(assetReference, out var pool))
            {
                pool = new AsyncAddressableGameObjectPool(assetReference);
                _pools[assetReference] = pool;
                return await pool.RequestAsync(cancellationToken);
            }

            return await assetReference.InstantiateAsync().ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask<GameObject> GetOrInstantiateAsync(AssetReferenceGameObject assetReference,
            Transform parent, int usePool, CancellationToken cancellationToken = default)
        {
            if (usePool > -1 && !_pools.TryGetValue(assetReference, out var pool))
            {
                pool = new AsyncAddressableGameObjectPool(assetReference);
                _pools[assetReference] = pool;
                return await pool.RentAsync(parent, cancellationToken);
            }

            return await assetReference.InstantiateAsync(parent).ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask<GameObject> GetOrInstantiateAsync(AssetReferenceGameObject assetReference,
            Vector3 position, Quaternion rotation, int usePool, CancellationToken cancellationToken = default)
        {
            if (usePool > -1 && !_pools.TryGetValue(assetReference, out var pool))
            {
                pool = new AsyncAddressableGameObjectPool(assetReference);
                _pools[assetReference] = pool;
                return await pool.RentAsync(position, rotation, cancellationToken);
            }

            return await assetReference.InstantiateAsync(position, rotation)
                .ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask<GameObject> GetOrInstantiateAsync(AssetReferenceGameObject assetReference,
            Vector3 position, Quaternion rotation, Transform parent, int usePool)
        {
            if (usePool > -1 && !_pools.TryGetValue(assetReference, out var pool))
            {
                pool = new AsyncAddressableGameObjectPool(assetReference);
                _pools[assetReference] = pool;
                return await pool.RentAsync(position, rotation, parent);
            }

            return await assetReference.InstantiateAsync(position, rotation, parent);
        }

        public void ReturnToPoolOrDestroy(AssetReferenceGameObject assetReference, GameObject spawnedGameObject,
            int destroy)
        {
            if (destroy > -1 && _pools.TryGetValue(assetReference, out var pool))
            {
                pool.Return(spawnedGameObject);
            }
            else
            {
                Debug.LogWarning($"Trying to return an object to a non-existent pool for asset: {assetReference}");
                Destroy(spawnedGameObject);
            }
        }

        public void ReturnToPoolOrDestroy(Pair<AssetReferenceGameObject, GameObject> assetReferenceGameObjectPair,
            int destroy)
        {
            ReturnToPoolOrDestroy(assetReferenceGameObjectPair.Key, assetReferenceGameObjectPair.Value, destroy);
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