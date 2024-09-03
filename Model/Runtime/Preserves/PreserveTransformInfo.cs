using System;
using UnityEngine;

namespace Soul.Model.Runtime.Preserves
{
    [Serializable]
    public class PreserveTransformInfo
    {
        public bool useLocal = true;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public Vector3 GetPosition(Transform transform)
        {
            return useLocal ? transform.TransformPoint(position) : position;
        }

        protected GameObject SetTransformInfo(GameObject gameObject)
        {
            var transform = gameObject.transform;
            SetTransformInfo(transform);
            return gameObject;
        }

        protected Transform SetTransformInfo(Transform transform)
        {
            if (useLocal) transform.SetLocalPositionAndRotation(position, rotation);
            else transform.SetPositionAndRotation(position, rotation);

            transform.localScale = scale;
            return transform;
        }

        protected void LoadValueFrom(Transform transform)
        {
            var t = transform;
            if (useLocal) t.GetLocalPositionAndRotation(out position, out rotation);
            else t.GetPositionAndRotation(out position, out rotation);
            scale = t.localScale;
        }
    }
}