using System;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Peoples.Workers;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class RecordWorker
    {
        public Pair<WorkerType, int> workerCount;
    }
}