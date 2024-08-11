using _Root.Scripts.Controller.Runtime.Tasks;
using _Root.Scripts.Model.Runtime.Tasks;
using _Root.Scripts.Model.Runtime.Workers;
using Pancake;
using Pancake.Common;

namespace _Root.Scripts.Controller.Runtime.Spawners.Managers
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