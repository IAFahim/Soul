using QuickEye.Utility;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Items
{
    [CreateAssetMenu(fileName = "crop", menuName = "Soul/Item/Create Crop")]
    public class Crop : Item, IWeight
    {
        public UnityTimeSpan growTime;
        public float Weight => weight;
    }
}