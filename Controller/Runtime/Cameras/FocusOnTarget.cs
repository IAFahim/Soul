using System;
using Soul.Controller.Runtime.Events;
using Soul.Model.Runtime.Pivots;
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
        public EventTransformFocus onTargetSet;
        public float range = 0.2f;
        public bool reached;
        private (Transform target, HorizontalRegion pivotMode, IFocusCallBack FocusCallBackOrgin) _eventData;


        private void OnEnable()
        {
            onTargetSet.AddListener(OnTargetInvoke);
        }

        private void OnTargetInvoke(
            (Transform target, HorizontalRegion pivotMode, IFocusCallBack FocusCallBackOrgin) eventData)
        {
            _eventData = eventData;
            SetTarget(_eventData.target);
            SetOffset(eventData.pivotMode);
        }

        private void OnDisable()
        {
            onTargetSet.RemoveListener(OnTargetInvoke);
        }

        public void SetTarget(Transform newTarget)
        {
            reached = false;
            _eventData.target = newTarget;
        }

        private void SetOffset(HorizontalRegion eventDataPivotMode)
        {
            offset.x = eventDataPivotMode switch
            {
                HorizontalRegion.Center => 0,
                HorizontalRegion.Left => 10f * ((float)Screen.width / Screen.height) + 2.6f,
                HorizontalRegion.Right => -10f * ((float)Screen.width / Screen.height) - 2.6f,
                _ => offset.x
            };
        }


        private void LateUpdate()
        {
            if (_eventData.target == null) return;
            var targetPosition = _eventData.target.position;
            var targetPositionWithOffset = targetPosition + offset;
            mainCamera.position =
                Vector3.SmoothDamp(mainCamera.position, targetPositionWithOffset, ref _velocity, smoothTime);

            if (Vector3.Distance(mainCamera.position, targetPositionWithOffset) < range)
            {
                if (!reached)
                {
                    reached = true;
                    _eventData.FocusCallBackOrgin.OnFocus();
                }
            }
            else
            {
                if (reached)
                {
                    _eventData.FocusCallBackOrgin.OnOutOfFocus();
                    _eventData.target = null;
                    reached = false;
                }
            }
        }
    }
}