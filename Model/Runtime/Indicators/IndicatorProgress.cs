using Alchemy.Inspector;
using Links.Runtime;
using Soul.Model.Runtime.Bars;
using Soul.Model.Runtime.PoolAbles;
using Soul.Model.Runtime.Times;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Model.Runtime.Indicators
{
    public class IndicatorProgress : PoolAbleComponent
    {
        [SerializeField] protected Bar bar;
        [SerializeField] protected bool onProgressCompleteReturn;

        [SerializeField] private ScriptableEventGetGameObject getCameraEvent;
        private GameObject _mainCamera;
        private bool _wasWaiting;

        [SerializeField] protected RealTimeIntervalTicker realTimeIntervalTicker;
        [FormerlySerializedAs("rangedStagedTicker")] [FormerlySerializedAs("stagedTicker")] [SerializeField] protected RangedTimer rangedTicker;

        protected virtual void Awake()
        {
            bar.Setup();
            realTimeIntervalTicker = new RealTimeIntervalTicker(this);
            rangedTicker = new RangedTimer(this);
        }

        

        private void OnEnable()
        {
            if (_mainCamera == null)
            {
                _mainCamera = getCameraEvent.Get();
                if (_mainCamera != null) OnCameraChange(_mainCamera);
            }

            if (_mainCamera != null) return;
            getCameraEvent.OnValueChange += OnCameraChange;
            _wasWaiting = true;
        }

        private void OnCameraChange(GameObject mainCamera)
        {
            _mainCamera = mainCamera;
            transform.rotation = _mainCamera.transform.rotation;
        }


        private void OnDisable()
        {
            if (_wasWaiting) getCameraEvent.OnValueChange -= OnCameraChange;
            _wasWaiting = false;
            realTimeIntervalTicker.Stop();
            // StopCoroutine(FillRoutine);
        }

        
        [Button]
        public void Setup(float fill, float duration, bool onCompleteReturn)
        {
            onProgressCompleteReturn = onCompleteReturn;
            realTimeIntervalTicker.Start(fill, duration, AfterSecondProgress, OnProgressCompleted);
        }

        [Button]
        public void TestStage(float fill, float duration, int totalStages)
        {
            rangedTicker.Start(fill, duration, totalStages, AfterSecondProgress);
        }

        private void AfterSecondProgress(int obj)
        {
            Debug.Log(obj);
        }

        private void AfterSecondProgress(float fraction)
        {
            bar.UpdateParams(fraction);
        }
        
        private void OnProgressCompleted()
        {
            if (onProgressCompleteReturn) ReturnToPool();
        }
    }
}