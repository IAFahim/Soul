using System;
using System.Collections.Generic;
using Links.Runtime;
using Pancake;
using Pancake.Pools;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.DragAndDrops;
using Soul.Model.Runtime.Inventories;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Presenter.Runtime.DragAndDrops
{
    public class DropPresentationManager : GameComponent
    {
        [SerializeField] public Transform containerTransform;
        [SerializeField] private CanvasGroup containerCanvasGroup;
        public PlayerInventoryReference playerInventoryReference;
        public GameObject dragAndDropPrefab;

        private Dictionary<Item, DragAndDropItem> _instantiateItemAndContainers = new();
        [SerializeField] private ScriptableEventGetGameObject getCameraEvent;
        private GameObject _mainCamera;
        private bool _wasWaiting;

        private void OnEnable()
        {
            containerCanvasGroup.alpha = 0;
            playerInventoryReference.inventory.OnItemChanged += InventoryOnOnItemChanged;
            if (_mainCamera == null)
            {
                _mainCamera = getCameraEvent.Get();
                if (_mainCamera != null) OnCameraChange(_mainCamera);
            }

            if (_mainCamera != null) return;
            getCameraEvent.OnValueChange += OnCameraChange;
            _wasWaiting = true;
        }

        private void OnCameraChange(GameObject mainCamera) => _mainCamera = mainCamera;

        private void OnDisable()
        {
            playerInventoryReference.inventory.OnItemChanged -= InventoryOnOnItemChanged;
        }

        private void InventoryOnOnItemChanged(InventoryChangeEventArgs<Item, int> changeEventArgs)
        {
            if (_instantiateItemAndContainers.TryGetValue(changeEventArgs.Key, out var instance))
            {
                instance.UpdateCount(changeEventArgs.NewAmount, GetInventoryItemLimit(changeEventArgs.Key));
            }
        }

        public void OnSelect(Transform selectedTransform)
        {
            ClearAll();
            if (!selectedTransform.TryGetComponent(out IDropAble<Item> dropAble) || !dropAble.CanDropNow)
            {
                CantDrop();
                return;
            }

            if (selectedTransform.TryGetComponent<IAllowedToDropReference<Item>>(out var allowedToDropReference))
            {
                var gameObjectWithCount = GetGameObjectForInventory(
                    dragAndDropPrefab, playerInventoryReference.inventory,
                    allowedToDropReference.ListOfAllowedToDrop
                );

                if (gameObjectWithCount.Count > 0)
                {
                    containerCanvasGroup.alpha = 1;
                    _instantiateItemAndContainers = SetupItemContainer(gameObjectWithCount);
                }
                else CantDrop();
            }
            else CantDrop();
        }

        private Dictionary<Item, DragAndDropItem> SetupItemContainer(List<(GameObject, Item, int)> gameObjectWithCount)
        {
            foreach (var (instance, item, count) in gameObjectWithCount)
            {
                var dragAndDrop = instance.GetComponent<DragAndDropItem>();
                dragAndDrop.Setup(_mainCamera.GetComponent<Camera>(), item, count,
                    GetInventoryItemLimit(item)
                );
                _instantiateItemAndContainers.Add(item, dragAndDrop);
            }

            return _instantiateItemAndContainers;
        }

        private int GetInventoryItemLimit(Item item) => playerInventoryReference.inventory.GetLimitValueOrDefault(item);


        private List<(GameObject, T, TV)> GetGameObjectForInventory<T, TV>(GameObject prefab,
            Inventory<T, TV> inventory, IList<T> allowedToDropList
        ) where T : Item where TV : IComparable<TV>, IEquatable<TV>
        {
            containerCanvasGroup.alpha = 1;
            List<(GameObject, T, TV)> dragContainers = new();
            foreach (var item in allowedToDropList)
            {
                if (!inventory.TryGetValue(item, out TV current)) continue;
                dragContainers.Add((prefab.Request(containerTransform), item, current));
            }

            return dragContainers;
        }


        private void CantDrop()
        {
            containerCanvasGroup.alpha = 0;
            ClearAll();
        }

        private void ClearAll()
        {
            if (_instantiateItemAndContainers == null) return;
            foreach (var itemAndGameObject in _instantiateItemAndContainers)
            {
                itemAndGameObject.Value.GameObject.Return();
            }
            _instantiateItemAndContainers.Clear();
        }
    }
}