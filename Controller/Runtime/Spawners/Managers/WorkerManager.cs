using Pancake;
using Soul.Controller.Runtime.Tasks;
using Soul.Model.Runtime.Tasks;
using Soul.Model.Runtime.Workers;

namespace Soul.Controller.Runtime.Spawners.Managers
{
    public class WorkerManager : GameComponent
    {
        public WorkerGroup workerGroup;
        public WorkerType defaultWorkerType;
        
        public GameTask MoveWorkersToWorkplace()
        {
            var task = new TaskGotoWork();
            // defaultWorkerType.asyncAssetReferenceGameObject.InstantiateAsync()
            return task;
        }
        
    }
}