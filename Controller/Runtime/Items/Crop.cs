using Alchemy.Inspector;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Items
{
    [CreateAssetMenu(fileName = "crop", menuName = "Soul/Item/Create Crop")]
    public class Crop : Item, IWeight
    {
        [Title("Crop")] [Range(0, 100)] [SerializeReference]
        public int weight = 1;
        public int Weight => weight;
    }
}