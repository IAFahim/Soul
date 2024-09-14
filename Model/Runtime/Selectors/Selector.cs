using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Pancake.MobileInput;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Soul.Model.Runtime.Selectors
{
    [Serializable]
    public class Selector
    {
        public float waitForDrag = .1f;
        [SerializeField] private Transform currentTransform;
        [SerializeField] private bool canSelect = true;
        [SerializeField] private UnityEvent<Transform> onSelected;
        public bool selectProcessRunning;
        private EventSystem _eventSystem;
        private CancellationToken _token;
        private TouchCamera _touchCamera;

        public void Subscribe(TouchCamera touchCamera, EventSystem eventSystem, CancellationToken tokenSource)
        {
            _touchCamera = touchCamera;
            _eventSystem = eventSystem;
            selectProcessRunning = false;
            _token = tokenSource;
            TouchInput.OnStartDrag += TouchInputOnStartDrag;
            TouchInput.OnStopDrag += TouchInputOnStopDrag;
            TouchInput.OnFingerDown += TouchInputOnFingerDown;
        }

        public void UnSubscribe()
        {
            TouchInput.OnStartDrag -= TouchInputOnStartDrag;
            TouchInput.OnStopDrag -= TouchInputOnStopDrag;
            TouchInput.OnFingerDown -= TouchInputOnFingerDown;
            selectProcessRunning = false;
            _touchCamera = null;
        }

        private void TouchInputOnStopDrag(Vector3 arg1, Vector3 arg2)
        {
            canSelect = true;
        }

        private void TouchInputOnStartDrag(Vector3 position, bool isLongTap)
        {
            canSelect = false;
        }

        private void TouchInputOnFingerDown(Vector3 screenPoint)
        {
            Select(screenPoint, _token).Forget();
        }

        private async UniTask Select(Vector3 screenPoint, CancellationToken token)
        {
            if (_eventSystem.IsPointerOverGameObject() || _eventSystem.currentSelectedGameObject != null)
            {
                _touchCamera.OnDragSceneObject();
                return;
            }

            if (selectProcessRunning) return;
            selectProcessRunning = true;
            await UniTask.WaitForSeconds(waitForDrag, cancellationToken: token);
            Selection(screenPoint, token);
        }

        private void Selection(Vector3 screenPoint, CancellationToken token)
        {
            selectProcessRunning = false;
            if (!canSelect) return;
            var ray = _touchCamera.Cam.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out var raycastHit))
            {
                var hitTransform = raycastHit.transform;
                if (currentTransform != null && currentTransform == hitTransform)
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

            token.ThrowIfCancellationRequested();
        }

        private void SelectInvoke(RaycastHit raycastHit, Transform hitTransform)
        {
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(hitTransform.gameObject);
#endif
            currentTransform = hitTransform;
            var selectedCallBacks = currentTransform.GetComponents<ISelectCallBack>();
            foreach (var receiver in selectedCallBacks) receiver.OnSelected(raycastHit);
            onSelected.Invoke(currentTransform);
        }

        private void DeSelectInvoke(RaycastHit raycastHit)
        {
            var deSelectedCallBacks = currentTransform.GetComponents<IDeSelectedCallBack>();
            foreach (var receiver in deSelectedCallBacks) receiver.OnDeSelected(raycastHit);
            currentTransform = null;
        }

        private void ReSelectedInvoke(Transform hitTransform, RaycastHit raycastHit)
        {
            var reSelectedCallBacks = hitTransform.GetComponents<IReSelectedCallBack>();
            foreach (var receiver in reSelectedCallBacks) receiver.OnReSelected(raycastHit);
            onSelected.Invoke(hitTransform);
        }
    }
}