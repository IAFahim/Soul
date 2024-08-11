using _Root.Scripts.Model.Runtime;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Control
{
    public class DisableGameObjectOnLocationSwitchEvent: MonoBehaviour
    {
        public GameObject[] gameObjects;
        public LocationSwitchEvent locationSwitchEvent;

        private void OnEnable()
        {
            locationSwitchEvent.AddListener(Method);
        }

        private void Method((PlaceEnum location, int index) arg0)
        {
            foreach (var o in gameObjects)
            {
                o.SetActive(false);
            }
        }

        private void OnDisable()
        {
            locationSwitchEvent.AddListener(Method);
        }
    }
}