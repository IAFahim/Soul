using System;
using Pancake.Common;
using QuickEye.Utility;
using UnityEngine;

namespace Soul.Model.Runtime.Times
{
    [Serializable]
    public struct TimeRequirement
    {
        public UnityTimeSpan timeSpan;
        public UnityTimeSpan TimeRequired => timeSpan;

        public TimeSpan RemainingTimeSpan(DateTime startTime, DateTime currentDateTime) =>
            currentDateTime.Subtract(startTime).Add(timeSpan);

        public float RemainingSeconds(DateTime startTime, DateTime currentDateTime) =>
            (float)currentDateTime.Subtract(startTime).Add(timeSpan).TotalSeconds;

        public static implicit operator float(TimeRequirement timeRequirement) =>
            (int)timeRequirement.timeSpan.TotalSeconds;

        public DelayHandle Start(Action onComplete, bool useRealTime)
        {
            float timeRequired = this;
            return App.Delay(timeRequired, onComplete, null, false, useRealTime);
        }

        public (bool completed, DelayHandle delayHandle) Resume(DateTime startTime, DateTime currentDateTime,
            Action onComplete, bool useRealTime)
        {
            float remainingSeconds = RemainingSeconds(startTime, currentDateTime);
            if (remainingSeconds < 0 || Mathf.Approximately(remainingSeconds, 0))
            {
                onComplete.Invoke();
                return (true, null);
            }

            return (false, App.Delay(remainingSeconds, onComplete, null, false, useRealTime));
        }
    }
}