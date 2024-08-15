using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Reactives;
using Soul.Model.Runtime.Workers;
using UnityEngine;

namespace Soul.Controller.Runtime.Inventories
{
    [CreateAssetMenu(fileName = "itemInventory", menuName = "Soul/Inventory/Create Inventory")]
    public class ItemInventoryReference : ScriptableObject
    {
        public ReactivePair<Currency, int> coins;
        public ReactivePair<Item, int> gems;
        public WorkerInventory workers;
        public ItemInventory inventory;
        public static implicit operator ItemInventory(ItemInventoryReference reference) => reference.inventory;
    }
}