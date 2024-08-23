using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;

namespace Soul.Controller.Runtime.Requirements
{
    public struct RequirementForProduction
    {
        public int workerCount;
        public Pair<Currency, int> currency;
        public Pair<Item, int>[] items;
    }
}