using _Root.Scripts.Model.Runtime;
using Alchemy.Inspector;
using Pancake;
using UnityEngine.Serialization;

namespace _Root.Scripts.Controller.Runtime.Control
{
    public class LocationSwitchControl : GameComponent
    {
        [FormerlySerializedAs("location")] public PlaceEnum place;
        public int locationSubIndex;
        public LocationSwitchEvent locationSwitchEvent;

        [Button]
        public void Trigger()
        {
            locationSwitchEvent.Trigger((place, locationSubIndex));
        }
    }
}