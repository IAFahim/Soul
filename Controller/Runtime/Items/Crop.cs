using Alchemy.Inspector;
using Pancake.Pools;
using Soul.Model.Runtime.Items;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Controller.Runtime.Items
{
    [CreateAssetMenu(fileName = "crop", menuName = "Soul/Item/Create Crop")]
    public class Crop : Item, IWeight
    {
        [Title("Crop")] [Range(0, 100)] [SerializeReference]
        public int weight = 1;

        [SerializeField] private AssetReferenceGameObject[] assetReferenceGameObjects;
        private AddressableGameObjectPool[] _meshStagePool;
        public int Weight => weight;
    }
}