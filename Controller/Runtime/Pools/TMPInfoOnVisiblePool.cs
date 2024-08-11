using _Root.Scripts.Model.Runtime.Containers;
using _Root.Scripts.Model.Runtime.Preserves;
using Pancake;
using Pancake.Pools;
using TMPro;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Pools
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
            _instantiatedTMP = preserveGameObject.Request(parent).GetComponent<TextMeshPro>();
            _instantiatedTMP.fontSize = fontSize;
            _instantiatedTMP.text = textProvidingComponent.ToString();
        }

        private void OnBecameInvisible()
        {
            if (_instantiatedTMP) _instantiatedTMP.gameObject.Return();
        }
    }
}