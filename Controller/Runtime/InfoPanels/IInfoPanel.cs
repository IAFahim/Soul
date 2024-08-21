using UnityEngine;

namespace Soul.Controller.Runtime.InfoPanels
{
    public interface IInfoPanel
    {
        public bool Setup(Transform mainCamera, Transform targetTransform);
        public GameObject GameObject { get; }
    }
}