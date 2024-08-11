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
            selector.Subscribe(touchCamera, eventSystem);
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