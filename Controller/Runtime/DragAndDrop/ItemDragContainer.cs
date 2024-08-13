using Pancake.Pools;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.DragAndDrop
{
    public class ItemDragContainer : DragContainer
    {
        [SerializeField] private TextMeshProUGUIFormat textMeshProUGUIFormat;
        [SerializeField] private Item currentItem;
        [SerializeField] private ItemInventoryReference itemInventoryReference;

        [SerializeField]
        private Transform currentSelectedTransform;

        private void Awake()
        {
            textMeshProUGUIFormat.StoreFormat();
        }


        public bool Setup(ItemInventoryReference inventoryReference, Item item, Transform selectedTransform)
        {
            itemInventoryReference = inventoryReference;
            currentItem = item;
            currentSelectedTransform = selectedTransform;

            float getAmount = GetAllowedItemCount();
            if (getAmount > 0)
            {
                textMeshProUGUIFormat.SetTextFloat(getAmount);
                return true;
            }

            GameObject.Return();
            return false;
        }


        private float GetAllowedItemCount()
        {
            if (itemInventoryReference.inventory.TryGetItem(currentItem, out var inventoryAmount))
            {
                float allowedWeight = AllowedWeight(currentSelectedTransform);
                float minAmount = Mathf.Min(inventoryAmount, allowedWeight);
                return Mathf.RoundToInt(minAmount / PointToWeight(currentItem));
            }

            return 0;
        }

        public float AllowedWeight(Transform otherTransform)
        {
            float allowedAmount = 0;
            if (otherTransform.TryGetComponent<IWeightCapacity>(out var weightCapacity))
            {
                allowedAmount = weightCapacity.WeightLimit;
            }

            return allowedAmount;
        }

        public float PointToWeight(Item item)
        {
            float pointToWeight = 1;
            if (item is IPointToWeightReference pointToWeightReference)
            {
                pointToWeight = pointToWeightReference.PointToWeight;
            }

            return pointToWeight;
        }

        public override void OnDragRayCast(bool isHit, RaycastHit rayCast)
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
                        dropAble.Drop(new[] { currentItem });
                    }
                }
            }
        }
    }
}