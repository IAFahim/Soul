using Alchemy.Inspector;
using LitMotion;
using LitMotion.Extensions;
using Pancake;
using Pancake.Common;
using Pancake.MobileInput;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Soul.Model.Runtime.DragAndDrops
{
    public abstract class DragAndDropSelectable : GameComponent, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private float moveSpeedLimit = 50;
        [SerializeField] private float goingBackToOffsetDuration = .3f;
        [SerializeField] private LayerMask raycastLayerMask = Physics.DefaultRaycastLayers; // Added layer mask field

        [DisableInEditMode] public bool isDragging;
        public Transform cardTransform;
        private Camera _mainCamera;
        public Vector3 offset = new(0, 25, 0);

        private Vector3 _lastFingerPos;

        private Vector3 FingerPos => TouchWrapper.Touch0.Position;

        protected abstract void OnDragStart(PointerEventData eventData, bool isHit, RaycastHit rayCast);
        protected abstract void OnDrag(PointerEventData eventData, bool isHit, RaycastHit rayCast);
        protected abstract void OnEndDrag(PointerEventData eventData, bool isHit, RaycastHit rayCast);

        private void OnEnable()
        {
            _mainCamera ??= Camera.main;
        }

        protected void Setup(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDrag(eventData, CastRayFinger0PosWorld(out var rayCastHit), rayCastHit);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            OnDragStart(eventData, CastRayFinger0PosWorld(out var rayCastHit), rayCastHit);
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            OnEndDrag(eventData, CastRayFinger0PosWorld(out var rayCastHit, true), rayCastHit);
            LMotion.Create(cardTransform.localPosition, offset, goingBackToOffsetDuration).BindToLocalPosition(cardTransform);
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
            return Physics.Raycast(ray, out raycastHit, Mathf.Infinity, raycastLayerMask); // Use layer mask
        }

        private Ray ScreenPointToRay(Vector3 fingerPos)
        {
            return _mainCamera.ScreenPointToRay(fingerPos);
        }
    }
}