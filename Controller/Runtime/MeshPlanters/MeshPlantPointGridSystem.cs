using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Root.Scripts.NPC_Ai.Runtime.WayPointGizmoTool;
using Cysharp.Threading.Tasks;
using LitMotion;
using Pancake;
using Pancake.Common;
using Pancake.Pools;
using Soul.Controller.Runtime.Grids;
using Soul.Model.Runtime.Tweens;
using Soul.Model.Runtime.Tweens.Scriptable;
using UnityEngine;
using Math = System.Math;

namespace Soul.Controller.Runtime.MeshPlanters
{
    [Serializable]
    public class MeshPlantPointGridSystem : GameComponent, ILoadComponent
    {
        public GridWayPointLimiter gridWayPointLimiter;
        public GameObject plantHolderPrefab;
        public List<GameObject> instances;
        public TweenSettingFactor tweenSettingFactor;
        public Vector3 sizeReference;
        public int levelReference;
        public int batchMultiplier = 5;

        public void Setup(int level)
        {
            levelReference = level;
        }
        
        public void Clear()
        {
            foreach (var instance in instances) instance.gameObject.Return();
        }

        public void Plant(AddressableGameObjectPool stage)
        {
            Clear();
            bool collectedSize = false;
            var pointGrid = gridWayPointLimiter.SpawnPoints(levelReference);
            for (var i = 0; i < pointGrid.Count; i++)
            {
                var point = pointGrid[i];
                var instance = stage.Request(point.position, point.rotation, Transform);
                instances.Add(instance);
                if (!collectedSize)
                {
                    sizeReference = instance.transform.localScale;
                    collectedSize = true;
                }
            }
        }

        public async UniTaskVoid ClearAsync()
        {
            int batch = levelReference * batchMultiplier;
            for (var i = 0; i < instances.Count; i++)
            {
                var tasks = new List<UniTask>();
                int batchEnd = Math.Min(i + batch, instances.Count);
                for (int j = i; j < batchEnd; j++) tasks.Add(Clear(j));
                i = batchEnd - 1;
                await UniTask.WhenAll(tasks);
            }
        }

        private async UniTask Clear(int index)
        {
            var instance = instances[index];
            await instance.transform.TweenPlayer(tweenSettingFactor, sizeReference).GetAwaiter();
            instance.gameObject.Return();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            gridWayPointLimiter.OnDrawGizmosSelected();
        }
#endif
        void ILoadComponent.OnLoadComponents()
        {
            Reset();
        }

        private void Reset()
        {
            gridWayPointLimiter.WayPoints = transform.GetComponent<WayPoints>();
        }
    }
}