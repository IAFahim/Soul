using Pancake;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;

namespace Soul.Controller.Runtime.Items
{
    [UnityEngine.CreateAssetMenu(fileName = "Item Required Show Event", menuName = "Soul/Event/Item Required Show")]
    public class EventShowItemRequired : Event<Pair<Item, int>[]>
    {
    }
}