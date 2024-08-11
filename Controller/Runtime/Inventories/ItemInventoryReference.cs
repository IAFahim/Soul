using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Inventories
{
    [CreateAssetMenu(fileName = "itemInventory", menuName = "Scriptable/Inventory/Create Inventory")]
    public class ItemInventoryReference : ScriptableObject
    {
        public ItemInventory inventory;
        public static implicit operator ItemInventory(ItemInventoryReference reference) => reference.inventory;
        public ItemInventory tempInventory;
    }
}