using System.Collections;
using Alchemy.Inspector;
using Links.Runtime;
using Pancake.Common;
using Soul.Model.Runtime.Bars;
using Soul.Model.Runtime.PoolAbles;
using UnityEngine;

namespace Soul.Model.Runtime.Indicators
{
    public class IndicatorProgress : PoolAbleComponent
    {
        [SerializeField] protected Bar bar;
        [SerializeField] protected bool onProgressCompleteReturn;
        [SerializeField] protected int passedDurationCeil;
        protected Coroutine FillRoutine;
        protected readonly WaitForSecondsRealtime SecondTick = new(1);
        
        [SerializeField] private ScriptableEventGetGameObject getCameraEvent;
        private GameObject _mainCamera;
        private bool _wasWaiting;

        protected virtual void Awake()
        {
            bar.Setup();
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
        }


        // fill = 0.335 duration = 500
        // secondFraction = 0.335 * 500 = 177.5
        // passedDurationCeil = 178
        // secondCompleteTime = 178 - 177.5 = 0.5
        // fractionProgress = 178 / 500 = 0.356
        [Button]
        public void Setup(float fill, float duration, bool onCompleteReturn)
        {
            onProgressCompleteReturn = onCompleteReturn;
            if (FillRoutine != null) StopCoroutine(FillRoutine);
            var secondFraction = fill * duration;
            passedDurationCeil = (int)Mathf.Ceil(secondFraction);
            var secondCompleteTime = passedDurationCeil - secondFraction;
            float fractionProgress = passedDurationCeil / duration;
            if (passedDurationCeil < duration)
            {
                App.Delay(secondCompleteTime, () =>
                {
                    bar.UpdateParams(fractionProgress);
                    FillRoutine = StartCoroutine(TickFill((int)duration));
                }, useRealTime: true);   
            }
            else
            {
                bar.UpdateParams(1);
                if (onProgressCompleteReturn) ReturnToPool();
            }
        }

        private IEnumerator TickFill(float duration)
        {
            while (passedDurationCeil < duration)
            {
                var progress = passedDurationCeil / duration;
                bar.UpdateParams(progress);
                yield return SecondTick;
                passedDurationCeil++;
            }

            bar.UpdateParams(1);
            if (onProgressCompleteReturn) ReturnToPool();
        }
    }
}