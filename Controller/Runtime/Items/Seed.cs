using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Items
{
    public class Seed : Item, IPointToWeightReference
    {
        [SerializeField] private float pointToWeight;

        public float PointToWeight
        {
            get => pointToWeight;
            set => pointToWeight = value;
        }
    }
}