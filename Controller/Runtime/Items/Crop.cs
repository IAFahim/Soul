using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Items
{
    [CreateAssetMenu(fileName = "crop", menuName = "Soul/Item/Create Crop")]
    public class Crop : Item, IWeight, IPlantStageMesh
    {
        [Range(0, 100)] [SerializeReference] public int weight = 1;
        [SerializeField] public Vector3 size = Vector3.one;
        public int Weight => weight;
        public Mesh[] stageMeshes;
        public Mesh[] StageMeshes => stageMeshes;
        public Vector3 Size => size;
    }
}