using System;
using Soul.Controller.Runtime.Requirements;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Buildings.Records
{
    [Serializable]
    public class CropFieldRecord
    {
        public int level;
        [FormerlySerializedAs("productionRequirement")] public RecordProduction recordProduction;
        public UpgradeRequirement upgradeRequirement;
    }
}