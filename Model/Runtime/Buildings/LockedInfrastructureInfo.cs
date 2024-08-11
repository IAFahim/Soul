using System;
using _Root.Scripts.Model.Runtime.Infos;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Model.Runtime.Buildings
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