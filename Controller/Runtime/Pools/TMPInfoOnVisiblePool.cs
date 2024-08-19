using Pancake;
using Pancake.Pools;
using Soul.Model.Runtime.Preserves;
using TMPro;
using UnityEngine;

namespace Soul.Controller.Runtime.Pools
{
    public class TMPInfoOnVisiblePool : GameComponent
    {
        public PreservePrefabAndTransform preserveGameObject;
        public Transform parent;
        public float fontSize;
        public Component textProvidingComponent;

        private TextMeshPro _instantiatedTMP;

        private void OnBecameVisible()
        {
            preserveGameObject.PoolOrInstantiate(parent, out var tmpGameObject);
            tmpGameObject.GetComponent<TextMeshPro>();
            _instantiatedTMP.fontSize = fontSize;
            _instantiatedTMP.text = textProvidingComponent.ToString();
        }

        private void OnBecameInvisible()
        {
            if (_instantiatedTMP) _instantiatedTMP.gameObject.Return();
        }
    }
}