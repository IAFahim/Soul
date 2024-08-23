using QuickEye.Utility;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Items
{
    [CreateAssetMenu(fileName = "seed", menuName = "Soul/Item/Create Seed")]
    public class Seed : Item, IPointToWeightReference
    {
        [SerializeField] private float pointToWeight = 20;
        public UnityTimeSpan growTime;

        public float PointToWeight
        {
            get => pointToWeight;
            set => pointToWeight = value;
        }
    }
}