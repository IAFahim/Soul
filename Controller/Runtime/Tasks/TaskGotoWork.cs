using System;
using _Root.Scripts.Model.Runtime.Tasks;
using _Root.Scripts.Model.Runtime.Workers;
using Alchemy.Inspector;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Tasks
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