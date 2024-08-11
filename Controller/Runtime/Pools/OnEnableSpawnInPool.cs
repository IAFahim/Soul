using _Root.Scripts.Model.Runtime.Preserves;
using Pancake;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Pools
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