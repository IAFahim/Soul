using System;
using Pancake;
using UnityEngine;

namespace Soul.Controller.Runtime.Cameras
{
    public class FocusOnTarget : MonoBehaviour
    {
        public Transform target;
        public Transform mainCamera;
        public float smoothTime = 0.3f;
        public Vector3 offset = new Vector3(20, 35, -10);

        private Vector3 _velocity = Vector3.zero;
        public Event<Transform> onTargetSet;

        private void OnEnable()
        {
            onTargetSet.AddListener(SetTarget);
        }
        
        private void OnDisable()
        {
            onTargetSet.RemoveListener(SetTarget);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            offset.x = 10f * ((float)Screen.width / Screen.height) + 2.6f;
        }


        private void LateUpdate()
        {
            if (target == null) return;
            var targetPosition = target.position;
            var targetPositionWithOffset = targetPosition + offset;
            mainCamera.position =
                Vector3.SmoothDamp(mainCamera.position, targetPositionWithOffset, ref _velocity, smoothTime);
        }
    }
}