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
        public Reactive<int> xpPreview;

        public WorkerInventory workerInventory;
        public int maxWorker = 2;
        public Reactive<int> workerPreview;

        public Reactive<Limit> weight;
        public Reactive<int> weightPreview;
        public static implicit operator ItemInventory(PlayerFarmReference reference) => reference.inventory;

        public static implicit operator WorkerInventory(PlayerFarmReference reference) => reference.workerInventory;

        [Button]
        public void Load()
        {
            inventory.Load();
            workerInventory.Load();
            coins.Load();
            gems.Load();
            levelXp.Load();
        }


        [Button]
        public void Save()
        {
            inventory.Save();
            workerInventory.Save();
            coins.Save();
            gems.Save();
            levelXp.Save();
        }
    }
}