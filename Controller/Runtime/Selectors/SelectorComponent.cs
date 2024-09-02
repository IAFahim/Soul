using Cysharp.Threading.Tasks;
using Pancake.Common;
using Pancake.MobileInput;
using Soul.Model.Runtime.Selectors;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Soul.Controller.Runtime.Selectors
{
    public class SelectorComponent : MonoBehaviour, ILoadComponent
    {
        public Selector selector;
        public TouchCamera touchCamera;
        public EventSystem eventSystem;

        private void OnEnable()
        {
            var token = this.GetCancellationTokenOnDestroy();
            selector.Subscribe(touchCamera, eventSystem, token);
            App.Delay(0.1f, () => selector.selectProcessRunning = false);
        }

        private void OnDisable()
        {
            selector.UnSubscribe();
        }

        private void Reset()
        {
            touchCamera = GetComponentInParent<TouchCamera>();
            eventSystem = FindAnyObjectByType<EventSystem>();
        }

        void ILoadComponent.OnLoadComponents()
        {
            Reset();
        }
    }
}