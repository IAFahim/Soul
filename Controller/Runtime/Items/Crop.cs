using Alchemy.Inspector;
using Pancake.Pools;
using Soul.Model.Runtime.Items;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Controller.Runtime.Items
{
    [CreateAssetMenu(fileName = "crop", menuName = "Soul/Item/Create Crop")]
    public class Crop : Item, IWeight, IPlantStage
    {
        [Title("Crop")] [Range(0, 100)] [SerializeReference]
        public int weight = 1;

        [SerializeField] private AssetReferenceGameObject[] assetReferenceGameObjects;
        private AddressableGameObjectPool[] _meshStagePool;
        public int Weight => weight;
        public AddressableGameObjectPool[] MeshStagePool => _meshStagePool;

        private void OnEnable()
        {
            if (assetReferenceGameObjects == null || assetReferenceGameObjects.Length == 0) return;
            _meshStagePool = new AddressableGameObjectPool[assetReferenceGameObjects.Length];
            for (var i = 0; i < _meshStagePool.Length; i++)
            {
                _meshStagePool[i] = new AddressableGameObjectPool(assetReferenceGameObjects[i]);
            }
        }
    }
}