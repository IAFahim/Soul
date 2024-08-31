using QuickEye.Utility;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.LookUpTables
{
    [CreateAssetMenu(fileName = "PriceLookUpTable", menuName = "Soul/LookUpTable/Price")]
    public class PriceLookUpTable : ScriptableObject
    {
        public UnityDictionary<Item, int> lookUpTable;

        public bool TryGetValue(Item key, out int value) => lookUpTable.TryGetValue(key, out value);
    }
}