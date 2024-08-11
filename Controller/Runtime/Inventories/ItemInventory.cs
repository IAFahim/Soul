using System;
using _Root.Scripts.Model.Runtime.Inventories;
using _Root.Scripts.Model.Runtime.Items;

namespace _Root.Scripts.Controller.Runtime.Inventories
{
    [Serializable]
    public class ItemInventory : Inventory<Item, int>
    {
        protected override int AddValues(int a, int b) => a + b;
        protected override int SubtractValues(int a, int b) => a - b;
    }
}