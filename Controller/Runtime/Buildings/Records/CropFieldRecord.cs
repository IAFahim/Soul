using System;
using Soul.Controller.Runtime.Records;

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