using System;
using Soul.Controller.Runtime.Upgrades;

namespace Soul.Controller.Runtime.Productions
{
    [Serializable]
    public class BuildingAndProductionRecord
    {
        public int level;
        public RecordProduction recordProduction;
        public RecordUpgrade recordUpgrade;
    }
}