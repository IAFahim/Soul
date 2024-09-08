using System;
using UnityEngine;

namespace Soul.Model.Runtime.Bars
{
    [Serializable]
    public class Bar
    {
        private static readonly int fill1 = Shader.PropertyToID("_Fill");
        [SerializeField] private MeshRenderer meshRenderer;
        MaterialPropertyBlock matBlock;
        
        public void Setup() {
            matBlock = new MaterialPropertyBlock();
        }
        
        public void UpdateParams(float fill) {
            meshRenderer.GetPropertyBlock(matBlock);
            matBlock.SetFloat(fill1, fill);
            meshRenderer.SetPropertyBlock(matBlock);
        }
    }
}