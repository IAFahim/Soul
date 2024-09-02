using Pancake.Pools;
using Soul.Model.Runtime.Interfaces;
using UnityEngine;

namespace Soul.Controller.Runtime.Items
{
    public interface IPlantStage
    {
        public AddressableGameObjectPool[] MeshStagePool { get; }
    }
}