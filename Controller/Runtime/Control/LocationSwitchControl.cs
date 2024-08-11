using Alchemy.Inspector;
using Pancake;
using Soul.Model.Runtime;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Control
{
    public class LocationSwitchControl : GameComponent
    {
        public PlaceEnum place;
        public int locationSubIndex;
        public LocationSwitchEvent locationSwitchEvent;

        [Button]
        public void Trigger()
        {
            locationSwitchEvent.Trigger((place, locationSubIndex));
        }
    }
}