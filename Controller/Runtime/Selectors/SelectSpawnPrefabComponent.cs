using _Root.Scripts.Model.Runtime.Preserves;
using _Root.Scripts.Model.Runtime.Selectors;
using Pancake;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Selectors
{
    public class SelectSpawnPrefabComponent : GameComponent, ISelectCallBack, IDeSelectedCallBack
    {
        public PreservePrefabAndTransform preserveGameObject;

        private GameObject _instantiated;

        public void OnSelected(RaycastHit selfRaycastHit)
        {
            _instantiated = preserveGameObject.Request(Transform);
        }

        public void OnDeSelected(RaycastHit otherRaycastHit)
        {
            _instantiated.Return();
        }
    }
}