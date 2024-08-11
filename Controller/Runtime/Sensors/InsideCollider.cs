using System.Collections.Generic;
using Pancake.Common;
using UnityEngine;

namespace Soul.Controller.Runtime.Sensors
{
    public class InsideCollider : Sensor
    {
        public List<Transform> objectsInside = new();
        
        private void OnTriggerEnter(Collider other)
        {
            if (layerMask.Contains(other.gameObject.layer)) objectsInside.Add(other.transform);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (layerMask.Contains(other.gameObject.layer)) objectsInside.Remove(other.transform);
        }
        
        public Bounds GetBounds()
        {
            var boxCollider = GetComponent<Collider>();
            return boxCollider.bounds;
        }
    }
}