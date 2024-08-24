using QuickEye.Utility;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Items
{
    [CreateAssetMenu(fileName = "seed", menuName = "Soul/Item/Create Seed")]
    public class Seed : Item, ITimeRequirement, IKgToCount
    {
        [SerializeField] private int pointToWeight = 20;

        [FormerlySerializedAs("growTime")] [SerializeField]
        private UnityTimeSpan requiredTime;

        public UnityTimeSpan RequiredTime => requiredTime;

        public int KgToPoint
        {
            get => pointToWeight;
            set => pointToWeight = value;
        }
    }
}