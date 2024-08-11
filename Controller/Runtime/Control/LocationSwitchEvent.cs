using Pancake;
using Soul.Model.Runtime;

namespace Soul.Controller.Runtime.Control
{
    public class LocationSwitchEvent : Event<(PlaceEnum location, int index)>
    {
    }
}