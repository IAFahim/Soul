using System;
using _Root.Scripts.Controller.Runtime.Inventories;
using _Root.Scripts.Controller.Runtime.UI;
using _Root.Scripts.Model.Runtime.Items;
using _Root.Scripts.Model.Runtime.Levels;
using Pancake;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Presenter.Runtime.UI
{
    public class ProfileInventoryManager : GameComponent, ILoadComponent
    {
        public ItemInventoryReference itemInventoryReference;

        [Header("Coin")] public Item coin;
        public TextMeshProUGUIFormat coinText;
        public TextMeshProUGUIFormat maxCoinText;
        public TextMeshProUGUIFormat coinGoingToBeModifiedText;
        public CanvasGroup coinGoingToBeModifiedCanvasGroup;

        [Header("Gem")] public Item gem;
        public TextMeshProUGUIFormat gemText;
        public TextMeshProUGUIFormat gemGoingToBeModifiedText;
        public CanvasGroup gemGoingToBeModifiedCanvasGroup;

        [Header("Worker")] public int worker;
        public TextMeshProUGUIFormat workerText;
        public TextMeshProUGUIFormat maxWorkerText;
        public TextMeshProUGUIFormat workerGoingToBeModifiedText;
        public CanvasGroup workerGoingToBeModifiedCanvasGroup;

        [Header("Weight")] [BarAttribute.Bar] public Vector2 weight;
        public TextMeshProUGUIFormat weightText;
        public TextMeshProUGUIFormat maxWeightText;
        public TextMeshProUGUIFormat weightGoingToBeModifiedText;
        public CanvasGroup weightGoingToBeModifiedCanvasGroup;

        [Header("Level And XP")] public Level level;
        public TextMeshProUGUIFormat levelText;
        public TextMeshProUGUIFormat xpGoingToBeAddedText;
        public CanvasGroup xpGoingToBeAddedCanvasGroup;

        private void Awake()
        {
            StoreFormat();
        }

        public void OnEnable()
        {
            itemInventoryReference.inventory.OnItemAddedOrIncreased += OnItemAddedOrIncreased;
            itemInventoryReference.tempInventory.OnItemAddedOrIncreased += OnTempItemAddedOrIncreased;
            itemInventoryReference.inventory.OnItemDecreased += OnItemDecreased;
            itemInventoryReference.tempInventory.OnItemDecreased += OnTempItemDecreased;
            SetAllAlpha(0);
        }

        public void OnDisable()
        {
            itemInventoryReference.inventory.OnItemAddedOrIncreased -= OnItemAddedOrIncreased;
            itemInventoryReference.tempInventory.OnItemAddedOrIncreased -= OnTempItemAddedOrIncreased;
            itemInventoryReference.inventory.OnItemDecreased -= OnItemDecreased;
            itemInventoryReference.tempInventory.OnItemDecreased -= OnTempItemDecreased;
        }

        private void Start()
        {
            if (itemInventoryReference.inventory.TryGetItem(coin, out var coinCount)) coinText.SetTextFloat(coinCount);
            if (itemInventoryReference.inventory.TryGetItem(gem, out var gemCount)) gemText.SetTextFloat(gemCount);
            workerText.SetTextFloat(worker);
            weightText.SetTextFloat(weight.x);
            maxWeightText.SetTextFloat(weight.y);
            levelText.SetTextFloat(level);
        }

        private void OnItemAddedOrIncreased(Item item, int amount, int count, bool isAdded)
        {
            if (item == coin)
            {
                coinText.SetTextFloat(count);
                maxCoinText.SetTextFloat(amount);
            }
            else if (item == gem)
            {
                gemText.SetTextFloat(count);
            }
            else if (item is IWeight weightedItem)
            {
                weight.x += weightedItem.Weight * amount;
                weightText.SetTextFloat(weight.x);
                maxWeightText.SetTextFloat(weight.y);
            }
        }

        private void OnItemDecreased(Item item, int amount, int count)
        {
            if (item == coin)
            {
                coinText.SetTextFloat(count);
            }
            else if (item == gem)
            {
                gemText.SetTextFloat(count);
            }
            else if (item is IWeight weightedItem)
            {
                weight.x -= weightedItem.Weight * amount;
                weightText.SetTextFloat(weight.x);
            }
        }

        private void OnTempItemAddedOrIncreased(Item item, int amount, int count, bool isAdded)
        {
            if (Mathf.Approximately(0, amount)) return;
            if (item == coin)
            {
                coinGoingToBeModifiedText.SetTextFloat(-amount);
                coinGoingToBeModifiedCanvasGroup.alpha = 1;
            }
            else if (item == gem)
            {
                gemGoingToBeModifiedText.SetTextFloat(-amount);
                gemGoingToBeModifiedCanvasGroup.alpha = 1;
            }
            else if (item is IWeight weightedItem)
            {
                weightGoingToBeModifiedText.SetTextFloat(weightedItem.Weight * amount);
                weightGoingToBeModifiedCanvasGroup.alpha = 1;
            }
        }

        private void OnTempItemDecreased(Item item, int amount, int count)
        {
            if (item == coin)
            {
                coinGoingToBeModifiedCanvasGroup.alpha = 0;
            }
            else if (item == gem)
            {
                gemGoingToBeModifiedCanvasGroup.alpha = 0;
            }
            else if (item is IWeight weightedItem)
            {
                weightGoingToBeModifiedCanvasGroup.alpha = 0;
            }
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