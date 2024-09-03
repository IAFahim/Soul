using Pancake.Pools;
using QuickEye.Utility;
using Soul.Controller.Runtime.Pools;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.LookUpTables
{
    [CreateAssetMenu(fileName = "ItemStageLookUpTable", menuName = "Soul/LookUpTable/ItemStage")]
    public class ItemPoolsLookupTable : ScriptableObject
    {
        public UnityDictionary<Item, AddressablePools> lookUpTable;

        public void MarkNotReady()
        {
            foreach (var kvp in lookUpTable)
            {
                kvp.Value.ready = false;
            }
        }

        public bool TryGetValue(Item key, out AddressableGameObjectPool[] pools)
        {
            if(lookUpTable.TryGetValue(key, out var value))
            {
                pools = value.GetStagePools();
                return true;
            }
            pools = null;
            return false;
        }
    }
}