using System;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Workers;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public class BasicRequirement
    {
        public bool isActive;
        public Item mainItem;
        public Workers workers;
    }
}