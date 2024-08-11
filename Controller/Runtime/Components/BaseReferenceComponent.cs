using Pancake;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Components
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public sealed class BaseReferenceComponent : GameComponent, IPoolCallbackReceiver
    {
        private bool _returnToPool;
        
        public void OnRequest()
        {
            _returnToPool = true;
        }

        public void OnReturn()
        {
            
        }

        public void ReturnToPoolOrDestroy()
        {
            if (_returnToPool) GameObject.Return();
            else Destroy(GameObject);
        }
    }
}