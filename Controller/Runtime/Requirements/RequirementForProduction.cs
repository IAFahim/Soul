using System;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;

namespace Soul.Controller.Runtime.Requirements
{
    [Serializable]
    public struct RequirementForProduction
    {
        public int workerCount;
        public float timeMultiplier;
        public Pair<Currency, int> currency;
        public int weightCapacity;
    }
}