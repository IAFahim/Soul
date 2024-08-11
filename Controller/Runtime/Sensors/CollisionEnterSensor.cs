using Pancake.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Soul.Controller.Runtime.Sensors
{
    public class CollisionEnterSensor: Sensor
    {
        public UnityEvent<Collision> onCollisionEnter;
        
        private void OnCollisionEnter(Collision other)
        {
            if (layerMask.Contains(other.gameObject.layer)) onCollisionEnter.Invoke(other);
        }
        
    }
}