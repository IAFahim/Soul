using Soul.Controller.Runtime.Inventories;
using UnityEngine;

namespace Soul.Controller.Runtime.DragAndDrop
{
    [CreateAssetMenu(fileName = "TempHold", menuName = "Soul/Inventory/TempHold")]
    public class TempHold : ScriptableObject
    {
        public ItemInventory inventory;
     }
}