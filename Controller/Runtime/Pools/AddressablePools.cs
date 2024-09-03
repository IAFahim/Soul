using System;
using Pancake.Pools;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Controller.Runtime.Pools
{
    [Serializable]
    public class AddressablePools
    {
        [SerializeField] public bool ready;
        [SerializeField] private AssetReferenceGameObject[] assetReferenceGameObjects;
        private AddressableGameObjectPool[] _pools;

        public void Setup()
        {
            if (assetReferenceGameObjects == null || assetReferenceGameObjects.Length == 0) return;
            _pools = new AddressableGameObjectPool[assetReferenceGameObjects.Length];
            for (var i = 0; i < _pools.Length; i++)
            {
                _pools[i] = new AddressableGameObjectPool(assetReferenceGameObjects[i]);
            }

            ready = true;
        }

        public AddressableGameObjectPool[] GetStagePools()
        {
            if (!ready) Setup();
            return _pools;
        }
    }
}