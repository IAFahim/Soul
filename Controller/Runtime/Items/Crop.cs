using QuickEye.Utility;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Items
{
    [CreateAssetMenu(fileName = "crop", menuName = "Soul/Item/Create Crop")]
    public class Crop : Item, IWeight
    {
        [Range(0, 100)] [SerializeReference] public float weight = 1;
        public UnityTimeSpan growTime;
        public float Weight => weight;
    }
}