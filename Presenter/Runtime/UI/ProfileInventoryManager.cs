using Pancake;
using Pancake.Common;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Inventories;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Limits;
using UnityEngine;

namespace Soul.Presenter.Runtime.UI
{
    public class ProfileInventoryManager : GameComponent, ILoadComponent
    {
        public PlayerInventoryReference playerInventoryReference;

        [Header("Coin")] public Currency coin;
        public TMPFormat coinText;
        public TMPFormat maxCoinText;
        public TMPFormat coinGoingToBeModifiedText;
        public CanvasGroup coinGoingToBeModifiedCanvasGroup;

        [Header("Gem")] public TMPFormat gemText;
        public TMPFormat gemGoingToBeModifiedText;
        public CanvasGroup gemGoingToBeModifiedCanvasGroup;

        [Header("Worker")] public int worker;
        public TMPFormat workerText;
        public TMPFormat maxWorkerText;
        public TMPFormat workerGoingToBeModifiedText;
        public CanvasGroup workerGoingToBeModifiedCanvasGroup;

        public TMPFormat weightText;
        public TMPFormat maxWeightText;
        public TMPFormat weightGoingToBeModifiedText;
        public CanvasGroup weightGoingToBeModifiedCanvasGroup;

        [Header("Level And XP")] public Level level;
        public TMPFormat levelText;
        public TMPFormat xpGoingToBeAddedText;
        public CanvasGroup xpGoingToBeAddedCanvasGroup;

        private void Awake()
        {
            StoreFormat();
        }

        public void OnEnable()
        {
            CalculateTotalWeightInInventoryAndShow();
            playerInventoryReference.inventory.OnItemChanged += InventoryOnOnItemChanged;
            playerInventoryReference.weight.OnChange += WeightOnOnChange;
            // playerInventoryReference.inventoryPreview.OnAddedOrIncreased += OnTempAddedOrIncreased;
            // playerInventoryReference.inventory.OnDecreased += OnDecreased;
            playerInventoryReference.inventoryPreview.OnInventoryCleared += OnAllTempItemClear;
            SetAllAlpha(0);
        }

        private void WeightOnOnChange(LimitIntStruct old, LimitIntStruct newValue)
        {
            Debug.Log($"Weight changed from {old.Current} to {newValue.Current}");
            weightText.SetTextInt(newValue);
        }

        private void InventoryOnOnItemChanged(InventoryChangeEventArgs<Item, int> changeEventArgs)
        {
            if (changeEventArgs.ChangeType == InventoryChangeType.Added ||
                changeEventArgs.ChangeType == InventoryChangeType.Increased)
            {
                OnAddedOrIncreased(changeEventArgs.Key, changeEventArgs.NewAmount, changeEventArgs.ChangeAmount);
            }
            else if (changeEventArgs.ChangeType == InventoryChangeType.Decreased)
            {
                OnDecreased(changeEventArgs.Key, changeEventArgs.ChangeAmount, changeEventArgs.NewAmount);
            }
        }


        public void OnDisable()
        {
            playerInventoryReference.inventory.OnItemChanged -= InventoryOnOnItemChanged;
            playerInventoryReference.weight.OnChange -= WeightOnOnChange;
            // playerInventoryReference.inventory.OnAddedOrIncreased -= OnAddedOrIncreased;
            // playerInventoryReference.inventoryPreview.OnAddedOrIncreased -= OnTempAddedOrIncreased;
            // playerInventoryReference.inventory.OnDecreased -= OnDecreased;
            playerInventoryReference.inventoryPreview.OnInventoryCleared -= OnAllTempItemClear;
        }

        private void Start()
        {
            coinText.SetTextInt(playerInventoryReference.coins);
            gemText.SetTextInt(playerInventoryReference.gems);
            workerText.SetTextInt(worker);
            weightText.SetTextInt(playerInventoryReference.weight.Value.Current);
            maxWeightText.SetTextInt(playerInventoryReference.weight.Value.Max);
            levelText.SetTextInt(level);
        }

        private void OnAddedOrIncreased(Item item, int newAmount, int changeAmount)
        {
            if (item is IWeight weightedItem)
            {
                int itemWeight = weightedItem.Weight * changeAmount;
                playerInventoryReference.weight.Value += itemWeight;
            }
        }

        private void OnDecreased(Item item, int newAmount, int changeAmount)
        {
            if (item is IWeight weightedItem)
            {
                int itemWeight = weightedItem.Weight * changeAmount;
                playerInventoryReference.weight.Value -= itemWeight;
            }
        }

        public void CalculateTotalWeightInInventoryAndShow()
        {
            int weight = 0;
            foreach (var item in playerInventoryReference.inventory.GetAll())
            {
                if (item.Key is IWeight weightedItem)
                {
                    weight += weightedItem.Weight * item.Value;
                }
            }

            int maxWeight = playerInventoryReference.weight.Value.Max;
            playerInventoryReference.weight.Value = new LimitIntStruct(weight, maxWeight);
            weightText.SetTextFloat(weight);
        }

        private void OnTempAddedOrIncreased(Item item, int amount, int count, bool isAdded)
        {
            if (item is IWeight weightedItem)
            {
                weightGoingToBeModifiedText.SetTextFloat(weightedItem.Weight * amount);
                weightGoingToBeModifiedCanvasGroup.alpha = 1;
            }
        }

        private void OnAllTempItemClear()
        {
            SetAllAlpha(0);
        }


        private void SetAllAlpha(float alpha)
        {
            coinGoingToBeModifiedCanvasGroup.alpha = alpha;
            gemGoingToBeModifiedCanvasGroup.alpha = alpha;
            weightGoingToBeModifiedCanvasGroup.alpha = alpha;
            workerGoingToBeModifiedCanvasGroup.alpha = alpha;
            xpGoingToBeAddedCanvasGroup.alpha = alpha;
        }

        private void StoreFormat()
        {
            coinText.StoreFormat();
            maxCoinText.StoreFormat();
            coinGoingToBeModifiedText.StoreFormat();
            gemText.StoreFormat();
            gemGoingToBeModifiedText.StoreFormat();
            workerText.StoreFormat();
            maxWorkerText.StoreFormat();
            workerGoingToBeModifiedText.StoreFormat();
            weightText.StoreFormat();
            maxWeightText.StoreFormat();
            weightGoingToBeModifiedText.StoreFormat();
            levelText.StoreFormat();
            xpGoingToBeAddedText.StoreFormat();
        }

        void ILoadComponent.OnLoadComponents()
        {
            StoreFormat();
        }
    }
}