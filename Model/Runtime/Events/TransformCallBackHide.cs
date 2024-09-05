using Soul.Model.Runtime.UIs;
using UnityEngine;

namespace Soul.Model.Runtime.Events
{
    public struct TransformCallBackHide
    {
        public Transform transform;
        public IHideCallBack hideCallBack;

        public TransformCallBackHide(Transform t, IHideCallBack callBack)
        {
            transform = t;
            hideCallBack = callBack;
        }
    }
}