using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Links.Runtime;
using LitMotion;
using LitMotion.Extensions;
using Pancake;
using Pancake.Pools;
using Soul.Controller.Runtime.Grids;
using Soul.Controller.Runtime.LookUpTables;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Tweens.Scriptable;
using UnityEngine;
using DelayType = Cysharp.Threading.Tasks.DelayType;

namespace Soul.Controller.Runtime.MeshPlanters
{
    [Serializable]
    public class MeshPlantPointGridSystem : GameComponent
    {
        [SerializeField] private GridWayPointLimiter gridWayPointLimiter;
        [SerializeField] private ItemPoolsLookupTable itemPoolsLookupTable;

        [SerializeField] private float height = 10;
        [SerializeField] private TweenSettingBaseScriptableObject tweenSetting;


        private Item _currentItem;
        private int _levelReference;
        private int _currentStage = -1;
        private AddressableGameObjectPool[] _stagePools;
        private List<List<GameObject>> _coredInstances;

        private void Awake()
        {
            gridWayPointLimiter.WayPoints = transform.GetComponent<IPositionsAndRotationsProvider>();
        }

        public void Setup(int level, Item item, float progress)
        {
            if (_currentItem != item) itemPoolsLookupTable.TryGetValue(item, out _stagePools);
            _currentItem = item;
            _levelReference = level;
            if (Mathf.Approximately(progress, 0))
            {
                Clear();
                Plant(_stagePools[_currentStage = 0]);
            }
            else if (Mathf.Approximately(progress, 1))
            {
                Clear();
                Plant(_stagePools[_currentStage = _stagePools.Length - 1]);
            }
            else
            {
                Clear();
                Plant(_stagePools[_currentStage = Mathf.FloorToInt(progress * _stagePools.Length)]);
            }
        }


        private void Plant(AddressableGameObjectPool stage)
        {
            _coredInstances = new List<List<GameObject>>();
            var pointGrid = gridWayPointLimiter.CoredPoints(_levelReference);
            foreach (var point in pointGrid)
            {
                var coredInstance = new List<GameObject>();
                foreach (var (position, rotation) in point)
                {
                    var instance = stage.Request(position, rotation, Transform);
                    coredInstance.Add(instance);
                }

                _coredInstances.Add(coredInstance);
            }
        }

        public void Clear()
        {
            if (_currentStage == -1) return;
            var pool = _stagePools[_currentStage];
            foreach (var coredInstance in _coredInstances)
            {
                foreach (var o in coredInstance)
                {
                    pool.Return(o);
                }
            }
        }

        public void Complete()
        {
            if (_currentStage == _stagePools.Length - 1) return;
            Clear();
            Plant(_stagePools[_currentStage = _stagePools.Length - 1]);
        }

        public async UniTask ClearAsync()
        {
            var pool = _stagePools[_currentStage];
            for (var i = 0; i < _coredInstances.Count; i++)
            {
                await ClearCoreAsync(pool, i);
            }
        }


        private async UniTask ClearCoreAsync(AddressableGameObjectPool pool, int index)
        {
            var coredInstance = _coredInstances[index];
            var duration = tweenSetting.duration;
            var ease = tweenSetting.ease;
            foreach (var o in coredInstance)
            {
                var x = o.transform.position.y;
                LMotion.Create(x, x + height, duration).WithEase(ease).BindToPositionY(o.transform);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration), DelayType.Realtime);

            foreach (var o in coredInstance)
            {
                pool.Return(o);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            gridWayPointLimiter.OnDrawGizmosSelected();
        }
#endif
        
    }
}