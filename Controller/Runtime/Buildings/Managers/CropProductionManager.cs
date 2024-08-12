using System;
using Pancake;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Workers;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings.Managers
{
    public class CropProductionManager : GameComponent, ISaveAble, ISingleDrop, IWeightCapacity
    {
        public const string KeyPrefix = "prod";
        public int capacity;
        public ItemInventoryReference inventoryReference;
        public Item productionItem;
        public int cropCount;
        public WorkerGroup worker;
        public Item queueItem;
        
        public Tuple<Item, int, WorkerGroup> ProductionItem => new(productionItem, cropCount, worker);
        public float WeightLimit => capacity;

        public void Add(Item[] items)
        {
            queueItem = items[0];
            inventoryReference.tempInventory.AddOrIncreaseItem(queueItem, (int)WeightLimit);
        }

        public void StartProduction()
        {
            if (queueItem == null) return;
            if (inventoryReference.inventory.TryGetItem(queueItem, out var amount))
            {
                productionItem = queueItem;
                if (amount > 0)
                {
                    cropCount = Mathf.Min(amount, capacity);
                }
            }
        }

        public void Save(string key)
        {
            // Data.Save(key + KeyPrefix, ProductionItem);
        }

        
    }
}