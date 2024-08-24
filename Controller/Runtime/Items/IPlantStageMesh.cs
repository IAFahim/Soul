using Soul.Model.Runtime.Interfaces;
using UnityEngine;

namespace Soul.Controller.Runtime.Items
{
    public interface IPlantStageMesh: ISize
    {
        public Mesh[] StageMeshes { get; } 
    }
}