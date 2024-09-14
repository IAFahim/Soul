using Soul.Controller.Runtime.Inventories.Peoples;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Limits;
using Soul.Model.Runtime.Reactives;
using UnityEngine;

namespace Soul.Controller.Runtime.Inventories
{
    [CreateAssetMenu(fileName = "itemInventory", menuName = "Soul/Player/Farm Reference")]
    public class PlayerFarmReference : ScriptableObject
    {
        public ReactivePair<Currency, int> coins;
        public ReactivePair<Currency, int> coinPreview;
        public ReactivePair<Item, int> gems;
        public ReactivePair<Item, int> gemsPreview;
        public ItemInventory inventory;
        public LevelXp levelXp;
        public Reactive<float> xpPreview;
    
        public WorkerInventory workerInventory;
        public WorkerInventory workerInventoryPreview;

        public Reactive<LimitIntStruct> weight;
        public Reactive<int> weightPreview;
        public static implicit operator ItemInventory(PlayerFarmReference reference) => reference.inventory;

        public static implicit operator WorkerInventory(PlayerFarmReference reference) =>
            reference.workerInventory;
        
    }
}