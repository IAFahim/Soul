using UnityEngine;

namespace Soul.Model.Runtime.Workers
{
    public class WorkerInventory : ScriptableObject
    {
        [SerializeReference] public IWorker worker;
    }
}