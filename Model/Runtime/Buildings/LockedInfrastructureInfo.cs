using System;
using Soul.Model.Runtime.Infos;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Model.Runtime.Buildings
{
    [Serializable]
    public class LockedInfrastructureInfo : ScriptableStringInfo
    {
        [SerializeField] private AssetReferenceGameObject lockedModel;
        [SerializeField] private AssetReferenceGameObject unlockedModel;
        
        public AssetReferenceGameObject LockedModel => lockedModel;
        public AssetReferenceGameObject UnlockedModel => unlockedModel;
    }
}