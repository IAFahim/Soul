using QuickEye.Utility;
using UnityEngine;

namespace Soul.Model.Runtime.Items
{
    public class ItemTimed : Item
    {
        [SerializeField] protected UnityTimeSpan requiredTime;
        public UnityTimeSpan RequiredTime => requiredTime;
    }
}