﻿using System.Collections.Generic;
using Pancake;
using Pancake.Pools;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.CustomList;
using Soul.Model.Runtime.Drops;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Presenter.Runtime.DragAndDrops
{
    public class DropPresentationController : GameComponent
    {
        [SerializeField] public Transform containerTransform;
        [SerializeField] private CanvasGroup containerCanvasGroup;

        public PlayerInventoryReference playerInventoryReference;

        public ItemDragAndDropContainer itemDragAndDropContainerPrefab;

        public List<ItemDragAndDropContainer> instantiateDragContainers;

        private void OnEnable()
        {
            containerCanvasGroup.alpha = 0;
        }

        public void OnSelect(Transform selectedTransform)
        {
            var (canDrop, currentAllowedThingsToDrop) = TryGetAllowedList<Item>(selectedTransform);
            if (canDrop)
            {
                CanDrop(currentAllowedThingsToDrop, selectedTransform);
            }
            else
            {
                CantDrop();
            }
        }


        private void CanDrop(ScriptableList<Item> allowedThingsToDrop, Transform selectedTransform)
        {
            containerCanvasGroup.alpha = 1;
            foreach (var item in allowedThingsToDrop)
            {
                var dragContainer =
                    itemDragAndDropContainerPrefab.GameObject.Request<ItemDragAndDropContainer>(containerTransform);
                if (dragContainer.Setup(playerInventoryReference, item, selectedTransform))
                {
                    instantiateDragContainers.Add(dragContainer);
                }
            }
        }

        private void CantDrop()
        {
            containerCanvasGroup.alpha = 0;
            foreach (var dragContainer in instantiateDragContainers)
            {
                dragContainer.GameObject.Return();
            }

            instantiateDragContainers.Clear();
            playerInventoryReference.inventoryPreview.Clear();
        }

        private (bool canDrop, ScriptableList<T> currentAllowedThingsToDrop) TryGetAllowedList<T>(
            Transform selectedTransform)
        {
            if (selectedTransform.TryGetComponent<IDropAble<T>>(out var dropAble))
            {
                if (dropAble.CanDropNow) return (true, dropAble.AllowedThingsToDrop);
            }

            return (false, null);
        }
    }
}