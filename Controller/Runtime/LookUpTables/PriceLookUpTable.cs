using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.LookUpTables;
using UnityEngine;

namespace Soul.Controller.Runtime.LookUpTables
{
    public class PriceLookUpTable : ScriptableObject
    {
        public LookUpTable<Item, int> lookUpTable;

        public bool TryGet(Item key, out int value) => lookUpTable.TryGet(key, out value);
    }
}