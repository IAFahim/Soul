using System.Collections.Generic;
using Pancake.Pools;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.CustomList;
using Soul.Model.Runtime.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.DragAndDrop
{
    public class SingleGetDropAbleManager : GetDropAbleManager<Item>
    {
        public ItemInventoryReference itemInventoryReference;
        [FormerlySerializedAs("itemDragContainer")] public ItemDragContainer itemDragContainerPrefab;
        public List<ItemDragContainer> instantiateDragContainers;

        public override void CanDrop(ScriptableList<Item> allowedThingsToDrop, Transform container)
        {
            foreach (var item in allowedThingsToDrop)
            {
                var dragContainer = itemDragContainerPrefab.GameObject.Request<ItemDragContainer>(container);
                if (dragContainer.Setup(itemInventoryReference, item))
                {
                    instantiateDragContainers.Add(dragContainer);
                }
            }
        }

        public override void CantDrop()
        {
            foreach (var dragContainer in instantiateDragContainers)
            {
                dragContainer.GameObject.Return();
            }
        }
    }
}