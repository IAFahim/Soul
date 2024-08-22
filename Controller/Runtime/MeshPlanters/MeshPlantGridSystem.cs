using System;
using Pancake;
using Soul.Controller.Runtime.Grids;
using UnityEngine;

namespace Soul.Controller.Runtime.MeshPlanters
{
    [Serializable]
    public class MeshPlantGridSystem : GameComponent
    {
        public GridWayPointLimiter gridWayPointLimiter;
        public GameObject plantPrefab;
        public GameObject[] instances;
        
        

        public void CreateInstances()
        {
        }

        public void Plant(Mesh mesh)
        {
            
        }

        private void OnDrawGizmosSelected()
        {
            gridWayPointLimiter.OnDrawGizmosSelected();
        }
    }
}