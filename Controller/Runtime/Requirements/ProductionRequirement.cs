using System;
using QuickEye.Utility;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Peoples.Workers;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class ProductionRequirement : RequirementBasicWithTime
    {
        public bool isProducing;
        public Item productionItem;
    }
}