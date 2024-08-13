using Pancake.Pools;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.DragAndDrop
{
    public class ItemDragAndDropContainer : DragAndDropContainer
    {
        [SerializeField] private TextMeshProUGUIFormat textMeshProUGUIFormat;
        [SerializeField] private Item currentItem;
        [SerializeField] private ItemInventoryReference itemInventoryReference;

        [SerializeField] private Transform currentSelectedTransform;

        private void Awake()
        {
            textMeshProUGUIFormat.StoreFormat();
        }


        public bool Setup(ItemInventoryReference inventoryReference, Item item, Transform selectedTransform)
        {
            itemInventoryReference = inventoryReference;
            currentItem = item;
            currentSelectedTransform = selectedTransform;

            int getAmount = GetAllowedItemCount();
            if (getAmount > 0)
            {
                textMeshProUGUIFormat.SetTextInt(getAmount);
                return true;
            }

            GameObject.Return();
            return false;
        }


        private int GetAllowedItemCount()
        {
            if (itemInventoryReference.inventory.TryGetItem(currentItem, out var inventoryAmount))
            {
                int allowedWeight = AllowedWeight(currentSelectedTransform);
                int minAmount = Mathf.Min(inventoryAmount, allowedWeight);
                return minAmount;
            }

            return 0;
        }

        public int AllowedWeight(Transform otherTransform)
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
                if (currentSelectedTransform == rayCast.transform) return;
                itemInventoryReference.tempInventory.RemoveItem(currentItem);
                currentSelectedTransform = rayCast.transform;
                if (rayCast.transform.TryGetComponent<IDropAble<Item>>(out var dropAble))
                {
                    if (dropAble.CanDropNow)
                    {
                        dropAble.HoverDrop(new[] { currentItem });
                    }
                }
            }
            else
            {
                itemInventoryReference.tempInventory.RemoveItem(currentItem);
            }
        }

        protected override void OnDragRayCastEnd(bool isHit, RaycastHit rayCast)
        {
            if (isHit)
            {
                itemInventoryReference.tempInventory.RemoveItem(currentItem);
                currentSelectedTransform = rayCast.transform;
                if (rayCast.transform.TryGetComponent<IDropAble<Item>>(out var dropAble))
                {
                    if (dropAble.CanDropNow)
                    {
                        dropAble.Drop(new[] { currentItem });
                    }
                }
            }
            else
            {
                itemInventoryReference.tempInventory.RemoveItem(currentItem);
            }
        }
    }
}