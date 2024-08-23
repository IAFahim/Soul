using Pancake;
using Pancake.Pools;
using Soul.Model.Runtime.Extensions;
using Soul.Model.Runtime.PoolAbles;
using Soul.Model.Runtime.Selectors;
using UnityEngine;

namespace Soul.Model.Runtime.PopupIndicators
{
    public abstract class PopUpIndicator : PoolAbleComponent, ISelectCallBack
    {
        public abstract void OnSelected(RaycastHit selfRayCastHit);
    }
}