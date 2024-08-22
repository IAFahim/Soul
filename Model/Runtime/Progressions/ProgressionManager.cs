using System;
using Pancake;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.SaveAndLoad;
using TrackTime;

namespace Soul.Model.Runtime.Progressions
{
    public abstract class ProgressionManager<TRecord> : GameComponent where TRecord : ITimeBased, IInProgression
    {
        public TRecord recordReference;
        public Level levelReference;
        public abstract UnityTimeSpan FullTimeRequirement { get; }

        protected ISaveAbleReference SaveAbleReference;
        protected DelayHandle DelayHandle;


        public UnityDateTime StartedAt => recordReference.StartedAt;
        public UnityTimeSpan TimeDiscount => recordReference.Discount;
        public UnityTimeSpan DiscountedTime => recordReference.GetTimeAfterDiscount(FullTimeRequirement);
        public UnityDateTime EndsAt => recordReference.EndsAt(FullTimeRequirement);
        public UnityTimeSpan TimeRemaining => recordReference.Remaining(FullTimeRequirement);
        public float Progress => recordReference.Progress(FullTimeRequirement);
        public bool IsComplete => recordReference.InProgression && recordReference.IsOver(FullTimeRequirement);


        protected virtual bool Setup(TRecord record, Level level, ISaveAbleReference saveAbleReference)
        {
            recordReference = record;
            levelReference = level;
            SaveAbleReference = saveAbleReference;
            TimerStart(false);
            return true;
        }

        public virtual bool TryStartProgression()
        {
            bool hasEnough = HasEnough();
            if (hasEnough)
            {
                ModifyRecordBeforeProgression();
                TimerStart(true);
            }

            return hasEnough;
        }

        public abstract bool HasEnough();

        protected virtual void ModifyRecordBeforeProgression()
        {
            recordReference.InProgression = true;
            recordReference.StartedAt = new UnityDateTime(DateTime.UtcNow);
            SaveAbleReference.Save();
        }
        
        protected virtual void ModifyRecordAfterProgression()
        {
            recordReference.InProgression = false;
            SaveAbleReference.Save();
        }

        private void TimerStart(bool starsNow)
        {
            if (IsComplete)
            {
                OnComplete();
                return;
            }

            float delay = starsNow ? (float)DiscountedTime.TotalSeconds : (float)TimeRemaining.TotalSeconds;
            DelayHandle = App.Delay(delay, OnComplete);
            Track.Start(name, delay);
        }

        public abstract void OnComplete();
    }
}