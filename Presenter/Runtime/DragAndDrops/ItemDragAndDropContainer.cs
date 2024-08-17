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

        [FormerlySerializedAs("_itemInventoryReference")] [DisableInEditMode, SerializeField]
        private PlayerInventoryReference playerInventoryReference;


        [DisableInEditMode, ShowInInspector] private Transform hitSelectedTransform;

        private void Awake()
        {
            dropCountText.StoreFormat();
            itemTotalAmountText.StoreFormat();
        }


        public bool Setup(PlayerInventoryReference inventoryReference, Item item, Transform selectedTransform)
        {
            playerInventoryReference = inventoryReference;
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
            if (playerInventoryReference.inventory.TryGet(currentItem, out var inventoryAmount))
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
                playerInventoryReference.inventoryPreview.Remove(currentItem);
                hitSelectedTransform = rayCast.transform;
                if (rayCast.transform.TryGetComponent<IDropAble<Item>>(out var dropAble))
                {
                    if (dropAble.CanDropNow)
                    {
                        dropAble.DropHovering(new[] { currentItem });
                        return;
                    }
                }
            }

            playerInventoryReference.inventoryPreview.Clear(true);
        }

        protected override void OnDragRayCastEnd(bool isHit, RaycastHit rayCast)
        {
            if (isHit)
            {
                playerInventoryReference.inventoryPreview.Remove(currentItem);
                hitSelectedTransform = rayCast.transform;
                if (rayCast.transform.TryGetComponent<IDropAble<Item>>(out var dropAble))
                {
                    if (dropAble.CanDropNow)
                    {
                        dropAble.TryDrop(new[] { currentItem });
                        TrySetText();
                    }
                }
            }

            playerInventoryReference.inventoryPreview.Clear(true);
        }
    }
}