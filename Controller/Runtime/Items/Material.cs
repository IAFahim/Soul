using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Items
{
    [CreateAssetMenu(fileName = "material", menuName = "Soul/Item/Create Material")]
    public class Material : Item, IWeight
    {
        [Range(0, 10)] public int weight = 1;
        public int Weight => weight;
    }
}