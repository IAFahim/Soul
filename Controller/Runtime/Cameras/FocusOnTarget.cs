using Soul.Model.Runtime.Events;
using Soul.Model.Runtime.UIs;
using UnityEngine;

namespace Soul.Controller.Runtime.Cameras
{
    public class FocusOnTarget : MonoBehaviour
    {
        public Transform mainCamera;
        public float smoothTime = 0.3f;
        public Vector3 offset = new(20, 35, -10);

        private Vector3 _velocity = Vector3.zero;
        public EventTransformCallBackHide onTargetSet;
        public float range = 0.2f;
        public bool reached;
        private (Transform transform, IHideCallBack hideCallBack) _eventCallBack;
        
        private void OnTargetInvoke((Transform transform, IHideCallBack hideCallBack) eventCallBack)
        {
            _eventCallBack = eventCallBack;
            SetTarget(eventCallBack.transform);
        }

        private void OnEnable()
        {
            onTargetSet.AddListener(OnTargetInvoke);
        }

        private void OnDisable()
        {
            onTargetSet.RemoveListener(OnTargetInvoke);
        }

        public void SetTarget(Transform newTarget)
        {
            reached = false;
            _eventCallBack.transform = newTarget;
            offset.x = 10f * ((float)Screen.width / Screen.height) + 2.6f;
        }


        private void LateUpdate()
        {
            if (_eventCallBack.transform == null) return;
            var targetPosition = _eventCallBack.transform.position;
            var targetPositionWithOffset = targetPosition + offset;
            mainCamera.position =
                Vector3.SmoothDamp(mainCamera.position, targetPositionWithOffset, ref _velocity, smoothTime);
            
            if (Vector3.Distance(mainCamera.position, targetPositionWithOffset) < range)
            {
                if (!reached)
                {
                    reached = true;
                }
            }
            else
            {
                reached = false;
                // _eventCallBack.hideCallBack.HideCallBack();
            }
        }
    }
}