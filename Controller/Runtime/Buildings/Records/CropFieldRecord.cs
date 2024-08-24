using System;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;

namespace Soul.Controller.Runtime.Buildings.Records
{
    [Serializable]
    public class CropFieldRecord
    {
        public int level;
        public RecordProduction recordProduction;
        public RecordUpgrade recordUpgrade;
    }
}