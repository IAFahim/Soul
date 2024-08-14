using Alchemy.Inspector;
using Pancake.Pools;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Items;
using UnityEngine;


namespace Soul.Presenter.Runtime.DragAndDrops
{
    public class ItemDragAndDropContainer : DragAndDropContainer
    {
        [SerializeField] private TextMeshProUGUIFormat dropCountText;
        [SerializeField] private TextMeshProUGUIFormat itemTotalAmountText;

        [DisableInEditMode, SerializeField] private Item currentItem;
        [DisableInEditMode, SerializeField] private ItemInventoryReference itemInventoryReference;

        [DisableInEditMode, ShowInInspector] private Transform hitSelectedTransform;

        private void Awake()
        {
            dropCountText.StoreFormat();
            itemTotalAmountText.StoreFormat();
        }


        public bool Setup(ItemInventoryReference inventoryReference, Item item, Transform selectedTransform)
        {
            itemInventoryReference = inventoryReference;
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
            if (itemInventoryReference.inventory.TryGetItem(currentItem, out var inventoryAmount))
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
                itemInventoryReference.tempInventory.RemoveItem(currentItem);
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

            itemInventoryReference.tempInventory.Clear(true);
        }

        protected override void OnDragRayCastEnd(bool isHit, RaycastHit rayCast)
        {
            if (isHit)
            {
                itemInventoryReference.tempInventory.RemoveItem(currentItem);
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

            itemInventoryReference.tempInventory.Clear(true);
        }
    }
}