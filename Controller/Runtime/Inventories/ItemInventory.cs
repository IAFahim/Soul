using System;
using System.Collections.Generic;
using QuickEye.Utility;
using Soul.Model.Runtime.Inventories;
using Soul.Model.Runtime.Items;

namespace Soul.Controller.Runtime.Inventories
{
    [Serializable]
    public class ItemInventory : Inventory<Item, int>
    {
        public UnityDictionary<Item, int> itemLimit;
        public int defaultLimit;

        public bool TryGetValue(Item key, out int current, out int limit)
        {
            if (itemLimit.TryGetValue(key, out limit)) return items.TryGetValue(key, out current);
            current = default;
            return false;
        }

        public bool TryGetMaxValue(Item key, out int limit)
        {
            if (itemLimit.TryGetValue(key, out limit)) return true;
            limit = defaultLimit;
            return false;
        }
        
        public int GetLimitValueOrDefault(Item key) => itemLimit.GetValueOrDefault(key, defaultLimit);

        protected override int AddValues(int a, int b) => a + b;
        protected override int SubtractValues(int a, int b) => a - b;
    }
}