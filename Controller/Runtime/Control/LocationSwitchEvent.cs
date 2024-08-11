using _Root.Scripts.Model.Runtime;
using Pancake;

namespace _Root.Scripts.Controller.Runtime.Control
{
    public class LocationSwitchEvent : Event<(PlaceEnum location, int index)>
    {
    }
}