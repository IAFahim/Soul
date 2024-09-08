using System;
using System.Collections;
using Alchemy.Inspector;
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

        private void Awake()
        {
            bar.Setup();
        }

        // fill = 0.335 duration = 500
        // secondFraction = 0.335 * 500 = 177.5
        // passedDurationCeil = 178
        // secondCompleteTime = 178 - 177.5 = 0.5
        // fractionProgress = 178 / 500 = 0.356
        [Button]
        public void Setup(float fill, float duration, bool onCompleteReturn)
        {
            if (FillRoutine != null) StopCoroutine(FillRoutine);
            var secondFraction = fill * duration;
            passedDurationCeil = (int)Mathf.Ceil(secondFraction);
            var secondCompleteTime = passedDurationCeil - secondFraction;
            float fractionProgress = passedDurationCeil / duration;
            App.Delay(secondCompleteTime, () =>
            {
                bar.UpdateParams(fractionProgress);
                FillRoutine = StartCoroutine(TickFill((int)duration));
            }, useRealTime: true);
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