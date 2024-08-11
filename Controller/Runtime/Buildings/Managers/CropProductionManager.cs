using System;
using _Root.Scripts.Controller.Runtime.Inventories;
using _Root.Scripts.Model.Runtime.Interfaces;
using _Root.Scripts.Model.Runtime.Items;
using _Root.Scripts.Model.Runtime.Workers;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Buildings.Managers
{
    public class CropProductionManager : GameComponent, ISaveAble, ISingleDrop, IWeightLimiter
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