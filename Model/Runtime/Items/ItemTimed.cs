using QuickEye.Utility;
using Soul.Model.Runtime.Interfaces;
using UnityEngine;

namespace Soul.Model.Runtime.Items
{
    public class ItemTimed : Item, ITimeRequirement
    {
        [SerializeField] protected UnityTimeSpan requiredTime;
        public UnityTimeSpan RequiredTime => requiredTime;
    }
}