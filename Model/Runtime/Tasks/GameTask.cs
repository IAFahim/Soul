using System;
using Pancake;
using Pancake.Common;
using QuickEye.Utility;

namespace _Root.Scripts.Model.Runtime.Tasks
{
    [Serializable]
    public class GameTask
    {
        public Optional<GameTask> nextTask;
        public UnityTimeSpan time;

        public virtual DelayHandle Start()
        {
            return App.Delay((int)time.TotalSeconds, OnTimeEndToCompleted, useRealTime: true);
        }

        public virtual void OnTimeEndToCompleted()
        {
            Completed();
        }
        
        public virtual void Completed()
        {
            if (nextTask) nextTask.Value.Start();
        }
    }
}