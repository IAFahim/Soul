using Pancake;
using UnityEngine;

namespace Soul.Controller.Runtime.FloatingUI
{
    public class LookAtCamera : GameComponent
    {
        [SerializeField] protected Transform cameraTransform;

        private void OnEnable()
        {
            if(cameraTransform == null) cameraTransform = Camera.main.transform;
            transform.rotation = cameraTransform.rotation;
        }
    }
}