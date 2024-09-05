using Pancake;
using Soul.Model.Runtime.UIs;
using UnityEngine;

namespace Soul.Model.Runtime.Events
{
    [CreateAssetMenu(fileName = "EventTransformRemoveCallBack",menuName = "Soul/Events/EventTransformRemoveCallBack")]
    public class EventTransformCallBackHide : Event<(Transform transform, IHideCallBack hideCallBack)>
    {
    }
}