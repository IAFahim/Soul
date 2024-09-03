using Soul.Model.Runtime.PoolAbles;
using Soul.Model.Runtime.Selectors;
using UnityEngine;

namespace Soul.Model.Runtime.PopupIndicators
{
    public abstract class PopUpIndicator : PoolAbleComponent, ISelectCallBack, IReSelectedCallBack
    {
        public virtual void OnReSelected(RaycastHit selfReRaycastHit)
        {
            OnSelected(selfReRaycastHit);
        }

        public abstract void OnSelected(RaycastHit selfRayCastHit);
    }
}