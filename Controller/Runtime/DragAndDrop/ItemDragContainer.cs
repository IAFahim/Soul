﻿using Pancake.Pools;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.DragAndDrop
{
    public class ItemDragContainer : DragContainer
    {
        [SerializeField] private TextMeshProUGUIFormat textMeshProUGUIFormat;
        [SerializeField] private Item currentItem;
        [SerializeField] private ItemInventoryReference itemInventoryReference;
        [SerializeField] private Transform selectedTransform;

        private void Awake()
        {
            textMeshProUGUIFormat.StoreFormat();
        }

        public bool Setup(ItemInventoryReference inventoryReference, Item item)
        {
            itemInventoryReference = inventoryReference;
            currentItem = item;
            if (itemInventoryReference.inventory.TryGetItem(item, out var amount))
            {
                textMeshProUGUIFormat.SetTextFloat(amount);
                return true;
            }

            GameObject.Return();
            return false;
        }

        public override void OnDragRayCast(bool isHit, RaycastHit rayCast)
        {
            if (isHit)
            {
                if (selectedTransform == rayCast.transform) return;
                itemInventoryReference.tempInventory.Clear(true);
                selectedTransform = rayCast.transform;
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