using System;
using UnityEngine;

namespace Soul.Model.Runtime.Movements.Looks
{
    [Serializable]
    public class LookAt
    {
        public Transform transform;
        public Transform targetTransform;
        
        [SerializeField] private Vector3 pivotOffset;
        [SerializeField] private Bounds bounds = new(Vector3.zero, new Vector3(140, 100, 100));
        

        public void Setup(Transform selfTransform, Transform target)
        {
            transform = selfTransform;
            this.targetTransform = target;
            bounds = new Bounds(FrontAndBaseClippedCenter, bounds.size);
        }

        
        public void Update()
        {
            if (IsAlignNeeded()) AlignCameraToTransform();
        }

        private Vector3 FrontAndBaseClippedCenter =>
            transform.position + new Vector3(0, bounds.size.y / 2, -bounds.size.z / 2);

        private bool IsAlignNeeded()
        {
            var cameraPosition = targetTransform.position;
            var contains = bounds.Contains(cameraPosition);
            return contains;
        }

        private void AlignCameraToTransform()
        {
            var cameraTransform = targetTransform;
            var forward = transform.position - cameraTransform.position + pivotOffset;
            var rotation = Quaternion.LookRotation(forward, Vector3.up);
            transform.rotation = rotation;
        }

        public virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            bounds.center = FrontAndBaseClippedCenter;
            if (targetTransform)
            {
                var position = targetTransform.transform.position;
                Gizmos.color = bounds.Contains(position) ? Color.green : Color.red;
                Gizmos.DrawSphere(position, 1);
            }

            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
