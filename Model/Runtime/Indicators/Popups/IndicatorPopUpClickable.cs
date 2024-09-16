using Soul.Model.Runtime.PoolAbles;
using Soul.Model.Runtime.Selectors;
using UnityEngine;

namespace Soul.Model.Runtime.Indicators.Popups
{
    public abstract class IndicatorPopUpClickable : PoolAbleComponent, ISelectCallBack
    {
        public abstract void OnSelected(RaycastHit selfRayCastHit);
    }
}