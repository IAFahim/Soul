using Pancake;
using Pancake.Pools;
using Soul.Model.Runtime.Preserves;
using Soul.Model.Runtime.Selectors;
using UnityEngine;

namespace Soul.Controller.Runtime.Selectors
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