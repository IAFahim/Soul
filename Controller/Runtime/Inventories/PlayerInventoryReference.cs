using Soul.Controller.Runtime.Inventories.Peoples;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Reactives;
using UnityEngine;

namespace Soul.Controller.Runtime.Inventories
{
    [CreateAssetMenu(fileName = "itemInventory", menuName = "Soul/Inventory/Create Inventory")]
    public class PlayerInventoryReference : ScriptableObject
    {
        public ReactivePair<Currency, int> coins;
        public ReactivePair<Item, int> gems;
        public ItemInventory inventory;
        public WorkerInventory workerInventory;
        public static implicit operator ItemInventory(PlayerInventoryReference reference) => reference.inventory;
    }
}