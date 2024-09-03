using UnityEngine;
using UnityEngine.EventSystems;

namespace Soul.Model.Runtime.DragAndDrops
{
    public abstract class DragAndDropSelectableController<TData> : DragAndDropSelectable
    {
        public TData data;
        private IDropAble<TData> _dropAble;
        private Transform _lastTransform;

        public virtual void Setup(Camera mainCamera, TData selectedData)
        {
            data = selectedData;
            _lastTransform = null;
            base.Setup(mainCamera);
        }

        private void InvokeDropCheck(Transform hitTransform)
        {
            _lastTransform = hitTransform;
            if (!hitTransform.TryGetComponent(out _dropAble))
            {
                NoDropAbleFound();
                return;
            }

            if (_dropAble.CanDropNow)
            {
                _dropAble.OnDrag(data);
                CanDrop();
            }
            else
            {
                DropBusy();
            }
        }

        protected override void OnDragStart(PointerEventData eventData, bool isHit, RaycastHit rayCast)
        {
            if (isHit)
            {
                InvokeDropCheck(rayCast.transform);
            }
            else
            {
                NoDropAbleFound();
                _lastTransform = null;
            }
        }

        protected override void OnDrag(PointerEventData eventData, bool isHit, RaycastHit rayCast)
        {
            if (isHit)
            {
                if (rayCast.transform == _lastTransform) return;
                InvokeDropCheck(rayCast.transform);
            }
            else if (_dropAble != null)
            {
                _dropAble.OnDragCancel();
                _lastTransform = null;
                _dropAble = null;
                NoDropAbleFound();
            }
        }

        protected override void OnEndDrag(PointerEventData eventData, bool isHit, RaycastHit rayCast)
        {
            if (isHit && _dropAble != null)
            {
                if (_dropAble.OnDrop(data))
                    OnSuccessfulDrop();
                else
                    OnDropFailed();
            }
            else
            {
                NoDropAbleFound();
            }

            _dropAble = null;
        }

        // Abstract methods for handling drop scenarios (Consider using Unity Events instead)

        protected abstract void CanDrop();
        protected abstract void DropBusy();
        protected abstract void OnSuccessfulDrop();
        protected abstract void OnDropFailed();
        protected abstract void NoDropAbleFound();
    }
}