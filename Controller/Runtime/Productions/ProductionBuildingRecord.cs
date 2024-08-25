using System;
using Soul.Controller.Runtime.Upgrades;

namespace Soul.Controller.Runtime.Productions
{
    [Serializable]
    public class ProductionBuildingRecord
    {
        public int level;
        public RecordProduction recordProduction;
        public RecordUpgrade recordUpgrade;
    }
}