using Soul.Model.Runtime.Interfaces;
using UnityEngine;

namespace Soul.Controller.Runtime.Items
{
    public interface IPlantStageMesh: ISizeReference
    {
        public Mesh[] StageMeshes { get; } 
    }
}