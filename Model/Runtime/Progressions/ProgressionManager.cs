using System;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.SaveAndLoad;
using Soul.Model.Runtime.TrackTime;

namespace Soul.Model.Runtime.Progressions
{
    [Serializable]
    public abstract class ProgressionManager<TRecord> where TRecord : ITimeBasedReference, IInProgression
    {
        public TRecord recordReference;
        protected DelayHandle DelayHandle;
        protected Level LevelReference;
        protected ISaveAbleReference SaveAbleReference;
        public abstract UnityTimeSpan FullTimeRequirement { get; }

        public UnityDateTime StartedAt => recordReference.Time.StartedAt;
        public UnityTimeSpan TimeDiscount => recordReference.Time.Discount;
        public UnityTimeSpan DiscountedTime => recordReference.Time.GetTimeAfterDiscount(FullTimeRequirement);
        public UnityDateTime EndsAt => recordReference.Time.EndsAt(FullTimeRequirement);
        public UnityTimeSpan TimeRemaining => recordReference.Time.Remaining(FullTimeRequirement);
        public float ProgressRatio => recordReference.Time.ProgressRatio(FullTimeRequirement);
        public bool IsComplete => recordReference.InProgression && recordReference.Time.IsOver(FullTimeRequirement);


        /// <summary>
        ///     Initializes the Progression with the provided data for first time
        /// </summary>
        protected virtual bool Setup(TRecord record, Level level, ISaveAbleReference saveAbleReference)
        {
            recordReference = record;
            LevelReference = level;
            SaveAbleReference = saveAbleReference;
            var canStart = !level.IsLocked && recordReference.InProgression;
            if (canStart) StartTimer(false);
            return canStart;
        }

        /// <summary>
        ///     Starts the production process.
        /// </summary>
        public virtual bool TryStartProgression()
        {
            var hasEnough = HasEnough();
            if (hasEnough)
            {
                ModifyRecordBeforeProgression();
                TakeRequirement();
                StartTimer(true);
            }

            return hasEnough;
        }

        protected abstract void TakeRequirement();

        /// <summary>
        ///     Checks if there are enough resources to start production.
        /// </summary>
        public abstract bool HasEnough();

        protected virtual void ModifyRecordBeforeProgression()
        {
            recordReference.InProgression = true;
            recordReference.Time.StartedAt = new UnityDateTime(DateTime.UtcNow);
            SaveAbleReference.Save();
        }


        protected void StartTimer(bool startsNow)
        {
            if (IsComplete)
            {
                OnTimerStart(1);
                OnComplete();
                return;
            }

            OnTimerStart(ProgressRatio);
            var delay = startsNow ? (float)DiscountedTime.TotalSeconds : (float)TimeRemaining.TotalSeconds;
            DelayHandle = App.Delay(delay, OnComplete, useRealTime: true);
            Track.Start(ToString(), delay);
        }


        public abstract void OnTimerStart(float progressRatio);

        public abstract void OnComplete();

        public void Cancel()
        {
            DelayHandle?.Cancel();
            recordReference.InProgression = false;
            SaveAbleReference.Save();
        }
    }
}