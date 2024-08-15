using System;
using Soul.Controller.Runtime.Requirements;

namespace Soul.Controller.Runtime.Buildings.Records
{
    [Serializable]
    public class CropFieldRecord
    {
        public int level;
        public ProductionRequirement productionRequirement;
        public UpgradeRequirement upgradeRequirement;
    }
}