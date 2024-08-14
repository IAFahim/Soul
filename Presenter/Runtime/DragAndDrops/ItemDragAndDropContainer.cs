using Alchemy.Inspector;
using Pancake.Pools;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Items;
using UnityEngine;
using UnityEngine.Serialization;


namespace Soul.Presenter.Runtime.DragAndDrops
{
    public class ItemDragAndDropContainer : DragAndDropContainer
    {
        [SerializeField] private TextMeshProUGUIFormat dropCountText;
        [SerializeField] private TextMeshProUGUIFormat itemTotalAmountText;

        [DisableInEditMode, SerializeField] private Item currentItem;

        [DisableInEditMode, SerializeField]
        private ItemInventoryReference _itemInventoryReference;

        [DisableInEditMode, SerializeField]
        private TempHold _tempHold;

        [DisableInEditMode, ShowInInspector] private Transform hitSelectedTransform;

        private void Awake()
        {
            dropCountText.StoreFormat();
            itemTotalAmountText.StoreFormat();
        }


        public bool Setup(ItemInventoryReference inventoryReference, TempHold tempHold, Item item,
            Transform selectedTransform)
        {
            _itemInventoryReference = inventoryReference;
            _tempHold = tempHold;
            currentItem = item;
            hitSelectedTransform = selectedTransform;
            if (!TrySetText())
            {
                GameObject.Return();
                return false;
            }

            return true;
        }

        private bool TrySetText()
        {
            (int oneDropAmount, int inventoryAmount) = GetAllowedCounts();
            if (oneDropAmount > 0)
            {
                dropCountText.SetTextInt(oneDropAmount);
                itemTotalAmountText.SetTextInt(inventoryAmount);
                return true;
            }

            return false;
        }


        private (int oneDropAmount, int inventoryAmount) GetAllowedCounts()
        {
            int oneDropAmount = 0;
            if (_itemInventoryReference.inventory.TryGetItem(currentItem, out var inventoryAmount))
            {
                int allowedWeight = AllowedWeight(hitSelectedTransform);
                oneDropAmount = Mathf.Min(inventoryAmount, allowedWeight);
            }

            return (oneDropAmount, inventoryAmount);
        }

        private int AllowedWeight(Transform otherTransform)
        {
            float allowedAmount = 0;
            if (otherTransform.TryGetComponent<IWeightCapacity>(out var weightCapacity))
            {
                allowedAmount = weightCapacity.WeightLimit;
            }

            return (int)allowedAmount;
        }

        protected override void OnDragRayCast(bool isHit, RaycastHit rayCast)
        {
            if (isHit)
            {
                if (hitSelectedTransform == rayCast.transform) return;
                _tempHold.inventory.RemoveItem(currentItem);
                hitSelectedTransform = rayCast.transform;
                if (rayCast.transform.TryGetComponent<IDropAble<Item>>(out var dropAble))
                {
                    if (dropAble.CanDropNow)
                    {
                        dropAble.HoverDrop(new[] { currentItem });
                        return;
                    }
                }
            }

            _tempHold.inventory.Clear(true);
        }

        protected override void OnDragRayCastEnd(bool isHit, RaycastHit rayCast)
        {
            if (isHit)
            {
                _tempHold.inventory.RemoveItem(currentItem);
                hitSelectedTransform = rayCast.transform;
                if (rayCast.transform.TryGetComponent<IDropAble<Item>>(out var dropAble))
                {
                    if (dropAble.CanDropNow)
                    {
                        dropAble.Drop(new[] { currentItem });
                        TrySetText();
                    }
                }
            }

            _tempHold.inventory.Clear(true);
        }
    }
}