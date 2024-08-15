using System;
using System.Collections.Generic;
using UnityEngine;

namespace Soul.Model.Runtime.Workers
{
    [Serializable]
    public class WorkerManager : ScriptableObject
    {
        [SerializeField]
        private List<Worker> workers = new List<Worker>();

        public void AddWorker(Worker newWorker)
        {
            if (!workers.Contains(newWorker))
            {
                workers.Add(newWorker);
            }
        }
        
        public void RemoveWorker(Worker worker)
        {
            if (workers.Contains(worker))
            {
                workers.Remove(worker);
            }
        }

        public Worker GetAvailableWorker()
        {
            return workers.Find(worker => worker.isAvailable);
        }

        public List<Worker> GetAllWorkers()
        {
            return new List<Worker>(workers);
        }
        
        public void AssignWorkToWorker(Worker worker)
        {
            if (workers.Contains(worker) && worker.isAvailable)
            {
                worker.AssignWork();
            }
        }

        public void CompleteWorkForWorker(Worker worker)
        {
            if (workers.Contains(worker) && !worker.isAvailable)
            {
                worker.CompleteWork();
            }
        }
    }
}