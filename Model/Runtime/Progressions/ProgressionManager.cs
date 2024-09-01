using System;
using Pancake;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.SaveAndLoad;
using TrackTime;

namespace Soul.Model.Runtime.Progressions
{
    public abstract class ProgressionManager<TRecord> : GameComponent
        where TRecord : ITimeBasedReference, IInProgression
    {
        public TRecord recordReference;
        public Level levelReference;
        public abstract UnityTimeSpan FullTimeRequirement { get; }

        protected ISaveAbleReference SaveAbleReference;
        protected DelayHandle DelayHandle;

        public UnityDateTime StartedAt => recordReference.Time.StartedAt;
        public UnityTimeSpan TimeDiscount => recordReference.Time.Discount;
        public UnityTimeSpan DiscountedTime => recordReference.Time.GetTimeAfterDiscount(FullTimeRequirement);
        public UnityDateTime EndsAt => recordReference.Time.EndsAt(FullTimeRequirement);
        public UnityTimeSpan TimeRemaining => recordReference.Time.Remaining(FullTimeRequirement);
        public float Progress => recordReference.Time.Progress(FullTimeRequirement);
        public bool IsComplete => recordReference.InProgression && recordReference.Time.IsOver(FullTimeRequirement);


        /// <summary>
        /// Initializes the Progression with the provided data for first time
        /// </summary>
        protected virtual bool Setup(TRecord record, Level level, ISaveAbleReference saveAbleReference)
        {
            recordReference = record;
            levelReference = level;
            SaveAbleReference = saveAbleReference;
            bool canStart = recordReference.InProgression;
            if (canStart) StartTimer(false);
            return canStart;
        }

        /// <summary>
        /// Starts the production process.
        /// </summary>
        public virtual bool TryStartProgression()
        {
            bool hasEnough = HasEnough();
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
        /// Checks if there are enough resources to start production.
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
            OnTimerStart(startsNow);
            if (IsComplete)
            {
                OnComplete();
                return;
            }

            float delay = startsNow ? (float)DiscountedTime.TotalSeconds : (float)TimeRemaining.TotalSeconds;
            DelayHandle = App.Delay(delay, OnComplete, useRealTime: true);
            Track.Start(name, delay);
        }

        public abstract void OnTimerStart(bool startsNow);

        public abstract void OnComplete();
    }
}