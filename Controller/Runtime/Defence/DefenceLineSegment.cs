using _Root.Scripts.Controller.Runtime.Sensors;
using Alchemy.Inspector;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Defence
{
    public class DefenceLineSegment : GameComponent
    {
        public InsideCollider insideCollider;
        public Bounds bound;
        public Transform entryPoint;
        
        [Button]
        public void UpdateBounds()
        {
            bound = insideCollider.GetBounds();
        }
        
        private void Reset()
        {
            insideCollider = GetComponent<InsideCollider>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bound.center, bound.size);
        }
    }
}