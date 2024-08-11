using System;
using Alchemy.Inspector;
using Soul.Model.Runtime.Tasks;
using Soul.Model.Runtime.Workers;
using UnityEngine;

namespace Soul.Controller.Runtime.Tasks
{
    [Serializable]
    public class TaskGotoWork : GameTask
    {
        public Worker[] workers;
        public Transform destination;
        
        [Button]
        public void SetUp(Worker[] workers, Transform destination)
        {
            this.workers = workers;
            this.destination = destination;
        }

        public override void OnTimeEndToCompleted()
        {
            foreach (var worker in workers) worker.StartWork(destination);
            base.OnTimeEndToCompleted();
        }
    }
}