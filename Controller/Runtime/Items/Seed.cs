using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Items
{
    [CreateAssetMenu(fileName = "seed", menuName = "Soul/Item/Create Seed")]
    public class Seed : Item, IKgToCount
    {
        [SerializeField] private int pointToWeight = 20;

        public int KgToPoint
        {
            get => pointToWeight;
            set => pointToWeight = value;
        }
    }
}