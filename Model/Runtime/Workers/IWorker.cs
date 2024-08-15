using System;

namespace Soul.Model.Runtime.Workers
{
    public interface IWorker
    {
        public int Count { get; set; }
        public event Action<IWorker, int, int> OnChange;
    }
}