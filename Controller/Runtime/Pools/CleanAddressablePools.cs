using Soul.Controller.Runtime.LookUpTables;
using UnityEngine;

namespace Soul.Controller.Runtime.Pools
{
    public class CleanAddressablePools : MonoBehaviour
    {
        [SerializeField] private ItemPoolsLookupTable itemPoolsLookupTable;
        
        private void Awake()
        {
            itemPoolsLookupTable.MarkNotReady();
        }
    }
}