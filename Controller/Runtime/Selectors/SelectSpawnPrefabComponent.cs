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

        public void OnSelected(RaycastHit selfRayCastHit)
        {
            preserveGameObject.PoolOrInstantiate(Transform, out _instantiated);
        }

        public void OnDeSelected(RaycastHit otherRayCastHit)
        {
            _instantiated.Return();
        }
    }
}