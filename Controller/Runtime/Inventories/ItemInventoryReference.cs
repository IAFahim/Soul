using UnityEngine;

namespace Soul.Controller.Runtime.Inventories
{
    [CreateAssetMenu(fileName = "itemInventory", menuName = "Soul/Inventory/Create Inventory")]
    public class ItemInventoryReference : ScriptableObject
    {
        public ItemInventory inventory;
        public static implicit operator ItemInventory(ItemInventoryReference reference) => reference.inventory;
        public ItemInventory tempInventory;
    }
}