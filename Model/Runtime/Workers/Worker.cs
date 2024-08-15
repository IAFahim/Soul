using System;

namespace Soul.Model.Runtime.Workers
{
    [Serializable]
    public class Worker : IWorker
    {
        public string workerName;
        public string role;
        public int productivity;
        public bool isAvailable;
        
        public void AssignWork()
        {
            isAvailable = false;
        }

        public void CompleteWork()
        {
            isAvailable = true;
            
        }
    }
}