using Alchemy.Inspector;
using LitMotion;
using LitMotion.Extensions;
using Pancake;
using Pancake.Common;
using Pancake.MobileInput;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Soul.Controller.Runtime.DragAndDrop
{
    public class DragContainer : GameComponent, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private float moveSpeedLimit = 50;
        [DisableInEditMode] public bool isDragging;
        public Transform cardTransform;
        private Camera _mainCamera;

        private Vector3 _lastFingerPos;

        private Vector3 FingerPos => TouchWrapper.Touch0.Position;

        private void OnEnable()
        {
            _mainCamera = Camera.main;
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragRayCast(CastRayFinger0PosWorld(out var rayCastHit), rayCastHit);
        }

        public virtual void OnDragRayCast(bool isHit, RaycastHit rayCast)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            LMotion.Create(cardTransform.localPosition, Vector3.zero, .3f).BindToLocalPosition(cardTransform);
        }

        private void Update()
        {
            if (isDragging) SmoothlyTranslate();
        }

        void SmoothlyTranslate()
        {
            Vector2 worldPosition = FingerPos;
            Vector2 position = cardTransform.position;
            Vector2 direction = (worldPosition - position).normalized;
            var pointerMoveSpeed = Vector2.Distance(position, worldPosition) / Time.deltaTime;
            Vector2 velocity = direction * moveSpeedLimit.Min(pointerMoveSpeed);
            cardTransform.Translate(velocity * Time.deltaTime);
        }


        private bool CastRayFinger0PosWorld(out RaycastHit raycastHit, bool useLasPosition = false)
        {
            Ray ray = useLasPosition ? ScreenPointToRay(_lastFingerPos) : ScreenPointToRay(_lastFingerPos = FingerPos);
            return Physics.Raycast(ray, out raycastHit);
        }

        private Ray ScreenPointToRay(Vector3 fingerPos)
        {
            return _mainCamera.ScreenPointToRay(fingerPos);
        }
    }
}