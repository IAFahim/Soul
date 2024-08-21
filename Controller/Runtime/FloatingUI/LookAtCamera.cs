using Pancake;
using UnityEngine;

namespace Soul.Controller.Runtime.FloatingUI
{
    public class LookAtCamera : GameComponent
    {
        [SerializeField] protected Transform cameraTransform;

        private void Start()
        {
            if(cameraTransform == null) cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            AlignCameraToTransform();
        }

        private void AlignCameraToTransform()
        {
            Transform.rotation = cameraTransform.rotation;
        }
    }
}