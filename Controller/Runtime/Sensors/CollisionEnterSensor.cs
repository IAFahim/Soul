using Pancake.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Root.Scripts.Controller.Runtime.Sensors
{
    public class CollisionEnterSensor: Sensor
    {
        [FormerlySerializedAs("onCollision")] public UnityEvent<Collision> onCollisionEnter;
        
        private void OnCollisionEnter(Collision other)
        {
            if (layerMask.Contains(other.gameObject.layer)) onCollisionEnter.Invoke(other);
        }
        
    }
}