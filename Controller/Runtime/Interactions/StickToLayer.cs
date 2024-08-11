using Pancake;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Interactions
{
    public class StickToLayer : GameComponent
    {
        public Rigidbody rb;
        public LayerMask stickToLayer;
        public float stickToLayerRadius = 0.1f;

        private void Update()
        {
            rb.isKinematic = Physics.CheckSphere(Transform.position, stickToLayerRadius, stickToLayer);
        }

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Transform.position, stickToLayerRadius);
        }
    }
}