using System;
using Soul.Model.Runtime.Inventories;
using Soul.Model.Runtime.Peoples.Workers;

namespace Soul.Controller.Runtime.Inventories.Peoples
{
    [Serializable]
    public class WorkerInventory: Inventory<WorkerType, int>
    {
        protected override int AddValues(int a, int b) => a + b;
        protected override int SubtractValues(int a, int b) => a - b;
    }
}