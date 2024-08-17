using System;
using Soul.Model.Runtime.Items;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class RecordProduction : RecordWorkerWithTime
    {
        public bool isProducing;
        public Item productionItem;
    }
}