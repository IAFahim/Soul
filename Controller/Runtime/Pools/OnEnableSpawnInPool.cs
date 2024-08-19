using Pancake;
using Soul.Model.Runtime.Preserves;
using UnityEngine;

namespace Soul.Controller.Runtime.Pools
{
    public class EnableSpawnInPool : GameComponent
    {
        [SerializeField] private PreservePrefabAndTransform preserveGameObject;
        [SerializeField] private GameObject spawnedGameObject;

        [SerializeField] private bool usedPool;

        private void OnEnable()
        {
            preserveGameObject.PoolOrInstantiate(Transform, out spawnedGameObject);
        }

        private void OnDisable()
        {
            if (spawnedGameObject) preserveGameObject.ReturnOrDestroy(spawnedGameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(preserveGameObject.GetPosition(Transform), preserveGameObject.scale);
        }
    }
}