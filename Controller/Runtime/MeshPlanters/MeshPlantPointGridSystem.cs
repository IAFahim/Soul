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
        public MeshFilter[] instances;
        public TweenSettingFactor tweenSettingFactor;
        public Vector3 sizeReference;
        public int levelReference;
        public int batchMultiplier = 5;

        public void Plant(int level, Mesh mesh, Vector3 size)
        {
            var pointGrid = gridWayPointLimiter.SpawnPoints(level);
            levelReference = level;
            sizeReference = size;
            instances = new MeshFilter[pointGrid.Count];
            for (var i = 0; i < pointGrid.Count; i++)
            {
                var point = pointGrid[i];
                var instance = plantHolderPrefab.Request(point.position, point.rotation, Transform);
                instance.transform.localScale = size;
                instances[i] = instance.GetComponent<MeshFilter>();
                instances[i].mesh = mesh;
            }

            Debug.Log("Planted");
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

        public async UniTaskVoid ClearAsync()
        {
            int batch = levelReference * batchMultiplier;
            for (var i = 0; i < instances.Length; i++)
            {
                var tasks = new List<UniTask>();
                int batchEnd = Math.Min(i + batch, instances.Length);
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