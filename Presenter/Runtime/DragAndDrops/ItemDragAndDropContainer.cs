using Alchemy.Inspector;
using Pancake.Pools;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Items;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Soul.Presenter.Runtime.DragAndDrops
{
    public class ItemDragAndDropContainer : DragAndDropContainer
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMPFormat dropCountText;
        [SerializeField] private TMPFormat itemTotalAmountText;

        [DisableInEditMode, SerializeField] private Item currentItem;

        [FormerlySerializedAs("_itemInventoryReference")] [DisableInEditMode, SerializeField]
        private PlayerInventoryReference playerInventoryReference;


        [ShowInInspector] private Transform _hitSelectedTransform;
        private (int singleDropAmount, int inventoryAmount) _inventoryCheckCache;

        private Pair<Item, int> DropPackage => new(currentItem, _inventoryCheckCache.singleDropAmount);


        private void Awake()
        {
            dropCountText.StoreFormat();
            itemTotalAmountText.StoreFormat();
        }


        public bool Setup(PlayerInventoryReference inventoryReference, Item item, Transform selectedTransform)
        {
            playerInventoryReference = inventoryReference;
            currentItem = item;
            _hitSelectedTransform = selectedTransform;
            return TryUpdateUI(item, out _);
        }

        private bool TryUpdateUI(Item item, out (int singleDropAmount, int inventoryAmount) inventoryCheck)
        {
            var hasEnough = TryGetSingleInventory(item, out inventoryCheck);
            _inventoryCheckCache = inventoryCheck;
            if (!hasEnough)
            {
                GameObject.Return();
                return false;
            }

            dropCountText.SetTextInt(inventoryCheck.singleDropAmount);
            itemTotalAmountText.SetTextInt(inventoryCheck.inventoryAmount);
            icon.sprite = item.Icon;

            return true;
        }

        private bool TryGetSingleInventory(Item item, out (int singleDropAmount, int inventoryAmount) inventoryCheck)
        {
            inventoryCheck = InventoryCheck(item);
            return inventoryCheck.singleDropAmount > 0;
        }


        private (int singleDropAmount, int inventoryAmount) InventoryCheck(Item item)
        {
            int oneDropAmount = 0;
            if (playerInventoryReference.inventory.TryGet(item, out var inventoryAmount))
            {
                int allowedWeight = AllowedWeight(_hitSelectedTransform);
                oneDropAmount = Mathf.Min(inventoryAmount, allowedWeight);
            }

            return (oneDropAmount, inventoryAmount);
        }

        private int AllowedWeight(Transform otherTransform)
        {
            float allowedAmount = 0;
            if (otherTransform.TryGetComponent<IWeightCapacityReference>(out var weightCapacity))
            {
                allowedAmount = weightCapacity.WeightCapacity;
            }

            return (int)allowedAmount;
        }

        protected override void OnDragRayCast(bool isHit, RaycastHit rayCast)
        {
            if (isHit)
            {
                if (_hitSelectedTransform == rayCast.transform) return;
                playerInventoryReference.inventoryPreview.Remove(currentItem);
                _hitSelectedTransform = rayCast.transform;
                if (rayCast.transform.TryGetComponent<IDropAble<Pair<Item, int>>>(out var dropAble))
                {
                    if (dropAble.CanDropNow)
                    {
                        dropAble.DropHovering(DropPackage);
                        return;
                    }
                }
            }

            playerInventoryReference.inventoryPreview.Clear();
        }

        protected override void OnDragRayCastEnd(bool isHit, RaycastHit rayCast)
        {
            if (isHit)
            {
                playerInventoryReference.inventoryPreview.Remove(currentItem);
                _hitSelectedTransform = rayCast.transform;
                if (rayCast.transform.TryGetComponent<IDropAble<Pair<Item, int>>>(out var dropAble))
                {
                    if (dropAble.CanDropNow)
                    {
                        dropAble.TryDrop(DropPackage);
                        TryUpdateUI(currentItem, out _);
                    }
                }
            }

            playerInventoryReference.inventoryPreview.Clear();
        }
    }
}