using System.Collections.Generic;
using Pancake;
using Pancake.Pools;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.Containers;
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
            var hasDropable = selectedTransform.TryGetComponent(out IDropAble<Pair<Item, int>> dropable);
            if (hasDropable && dropable.CanDropNow)
            {
                CanDrop(selectedTransform);
            }
            else
            {
                CantDrop();
            }
        }


        private void CanDrop(Transform selectedTransform)
        {
            containerCanvasGroup.alpha = 1;
            var allowedThingsToDropReference = selectedTransform.GetComponent<IAllowedToDropReference<Item>>();
            foreach (var item in allowedThingsToDropReference.ListOfAllowedToDrop)
            {
                var dragContainer = itemDragAndDropContainerPrefab.GameObject.Request<ItemDragAndDropContainer>(containerTransform);
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
    }
}