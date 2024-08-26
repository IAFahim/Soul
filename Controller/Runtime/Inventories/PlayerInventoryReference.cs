using Soul.Controller.Runtime.Inventories.Peoples;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Limits;
using Soul.Model.Runtime.Reactives;
using UnityEngine;

namespace Soul.Controller.Runtime.Inventories
{
    [CreateAssetMenu(fileName = "itemInventory", menuName = "Soul/Inventory/Create Inventory")]
    public class PlayerInventoryReference : ScriptableObject
    {
        public ReactivePair<Currency, int> coins;
        public ReactivePair<Currency, int> coinPreview;
        public ReactivePair<Item, int> gems;
        public ReactivePair<Item, int> gemsPreview;
        public ItemInventory inventory;

        public WorkerInventory workerInventory;
        public WorkerInventory workerInventoryPreview;

        public Reactive<LimitIntStruct> weight;
        public Reactive<int> weightPreview;
        public static implicit operator ItemInventory(PlayerInventoryReference reference) => reference.inventory;

        public static implicit operator WorkerInventory(PlayerInventoryReference reference) =>
            reference.workerInventory;
    }
}