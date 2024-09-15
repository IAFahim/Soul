using Alchemy.Inspector;
using Soul.Controller.Runtime.Inventories.Peoples;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Limits;
using Soul.Model.Runtime.Reactives;
using UnityEngine;

namespace Soul.Controller.Runtime.Inventories
{
    [CreateAssetMenu(fileName = "itemInventory", menuName = "Soul/Player/Farm Reference")]
    public class PlayerFarmReference : ScriptableObject
    {
        public ReactiveSaveAble<int> coins;
        public Reactive<int> coinPreview;
        public ReactiveSaveAble<int> gems;
        public Reactive<int> gemsPreview;
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
        
        [Button]
        public void Load()
        {
            inventory.Load();
            workerInventory.Load();
            workerInventoryPreview.Load();
            coins.Load();
            gems.Load();
        }

        private void OnEnable()
        {
            Load();
        }
        
        [Button]
        public void Save()
        {
            inventory.Save();
            workerInventory.Save();
            workerInventoryPreview.Save();
            coins.Save();
            gems.Save();
        }
    }
}