using System;
using Cysharp.Threading.Tasks;
using Pancake.MobileInput;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Soul.Model.Runtime.Selectors
{
    [Serializable]
    public class Selector
    {
        public float waitForDrag = .1f;
        [SerializeField] private Transform currentTransform;
        [SerializeField] private bool canSelect = true;
        [SerializeField] private UnityEngine.Events.UnityEvent<Transform> onSelected;
        private TouchCamera _touchCamera;
        private EventSystem _eventSystem;

        public void Subscribe(TouchCamera touchCamera, EventSystem eventSystem)
        {
            _touchCamera = touchCamera;
            _eventSystem = eventSystem;
            TouchInput.OnStartDrag += TouchInputOnOnStartDrag;
            TouchInput.OnStopDrag += TouchInputOnOnStopDrag;
            TouchInput.OnFingerDown += TouchInputOnOnFingerDown;
        }

        private void TouchInputOnOnStopDrag(Vector3 arg1, Vector3 arg2)
        {
            canSelect = true;
        }

        private void TouchInputOnOnStartDrag(Vector3 position, bool isLongTap)
        {
            canSelect = false;
        }

        private void TouchInputOnOnFingerDown(Vector3 screenPoint)
        {
            Select(screenPoint).Forget();
        }

        private async UniTask Select(Vector3 screenPoint)
        {
            if (_eventSystem.IsPointerOverGameObject() || _eventSystem.currentSelectedGameObject != null)
            {
                _touchCamera.OnDragSceneObject();
                return;
            }

            await UniTask.WaitForSeconds(waitForDrag);
            if (!canSelect) return;
            var ray = _touchCamera.Cam.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out var raycastHit))
            {
                var hitTransform = raycastHit.transform;
                if (currentTransform == hitTransform)
                {
                    ReSelectedInvoke(hitTransform, raycastHit);
                }
                else if (currentTransform != hitTransform)
                {
                    if (currentTransform) DeSelectInvoke(raycastHit);
                    SelectInvoke(raycastHit, hitTransform);
                }
            }
            else if (currentTransform)
            {
                DeSelectInvoke(raycastHit);
            }
        }

        private void SelectInvoke(RaycastHit raycastHit, Transform hitTransform)
        {
            currentTransform = hitTransform;
            onSelected.Invoke(currentTransform);
            var selectedCallBacks = currentTransform.GetComponentsInChildren<ISelectCallBack>();
            foreach (var receiver in selectedCallBacks) receiver.OnSelected(raycastHit);
        }

        private void DeSelectInvoke(RaycastHit raycastHit)
        {
            var deSelectedCallBacks = currentTransform.GetComponentsInChildren<IDeSelectedCallBack>();
            foreach (var receiver in deSelectedCallBacks) receiver.OnDeSelected(raycastHit);
            currentTransform = null;
        }

        private static void ReSelectedInvoke(Transform hitTransform, RaycastHit raycastHit)
        {
            var reSelectedCallBacks = hitTransform.GetComponentsInChildren<IReSelectedCallBack>();
            foreach (var receiver in reSelectedCallBacks) receiver.OnReSelected(raycastHit);
        }

        public void UnSubscribe()
        {
            TouchInput.OnFingerDown -= TouchInputOnOnFingerDown;
            TouchInput.OnStartDrag += TouchInputOnOnStartDrag;
            TouchInput.OnStopDrag += TouchInputOnOnStopDrag;
            _touchCamera = null;
        }
    }
}