using Pancake;
using Pancake.Pools;
using Soul.Model.Runtime.Preserves;
using UnityEngine;

namespace Soul.Controller.Runtime.Pools
{
    public class OnEnableSpawnInPool : GameComponent
    {
        [SerializeField] private PreservePrefabAndTransform preserveGameObject;
        [SerializeField] private GameObject spawnedGameObject;

        private void OnEnable()
        {
            spawnedGameObject = preserveGameObject.Request(Transform);
        }

        private void OnDisable()
        {
            if (spawnedGameObject) spawnedGameObject.Return();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(preserveGameObject.GetPosition(Transform), preserveGameObject.scale);
        }
    }
}