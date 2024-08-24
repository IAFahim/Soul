using System;
using Pancake;
using Pancake.Pools;
using Soul.Controller.Runtime.Grids;
using UnityEngine;

namespace Soul.Controller.Runtime.MeshPlanters
{
    [Serializable]
    public class MeshPlantPointGridSystem : GameComponent
    {
        public GridWayPointLimiter gridWayPointLimiter;
        public GameObject plantHolderPrefab;
        public MeshFilter[] instances;

        public void Plant(int level, Mesh mesh, Vector3 size)
        {
            var pointGrid = gridWayPointLimiter.SpawnPoints(level);
            instances = new MeshFilter[pointGrid.Count];
            for (var i = 0; i < pointGrid.Count; i++)
            {
                var point = pointGrid[i];
                var instance = plantHolderPrefab.Request(point.position, point.rotation,Transform);
                instance.transform.localScale = size;
                instances[i] = instance.GetComponent<MeshFilter>();
                instances[i].mesh = mesh;
            }
        }

        public void ChangeMesh(Mesh mesh)
        {
            foreach (var instance in instances)
            {
                instance.mesh = mesh;
                
            }
        }

        public void Clear()
        {
            foreach (var instance in instances) instance.gameObject.Return();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            gridWayPointLimiter.OnDrawGizmosSelected();
        }
#endif
    }
}