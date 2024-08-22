using System;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Peoples.Workers;

namespace Soul.Model.Runtime.Records
{
    [Serializable]
    public struct RecordWorker
    {
        public Pair<WorkerType, int> typeAndCount;
        public void Set(int amount)
        {
            typeAndCount.Value = amount;
        }
        
        public int Get()
        {
            return typeAndCount.Value;
        }
    }
}