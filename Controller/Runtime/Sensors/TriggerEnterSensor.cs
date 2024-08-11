using Pancake.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Soul.Controller.Runtime.Sensors
{
    public class TriggerEnterSensor : Sensor
    {
        public UnityEvent<Collider> onTriggerEnter;
        
        private void OnTriggerEnter(Collider other)
        {
            if (layerMask.Contains(other.gameObject.layer)) onTriggerEnter.Invoke(other);
        }
    }
}