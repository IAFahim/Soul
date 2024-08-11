using Pancake;
using UnityEngine;

namespace Soul.Model.Runtime.Workers
{
    public class Worker : GameComponent
    {
        [SerializeField] private bool isWorking;
        public bool IsWorking
        {
            get => isWorking;
            set => isWorking = value;
        }

        public void StartWork(Transform destination)
        {
            if (!IsWorking) Transform.position = destination.position;
            IsWorking = true;
        }
    }
}