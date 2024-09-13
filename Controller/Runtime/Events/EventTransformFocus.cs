using Pancake;
using Soul.Model.Runtime.Pivots;
using Soul.Model.Runtime.UIs;
using UnityEngine;

namespace Soul.Controller.Runtime.Events
{
    [CreateAssetMenu(fileName = "EventTransformRemoveCallBack", menuName = "Soul/Events/EventTransformRemoveCallBack")]
    public class EventTransformFocus : Event<(Transform target, HorizontalRegion pivotMode, IFocusCallBack hideCallBack)>
    {
    }
}