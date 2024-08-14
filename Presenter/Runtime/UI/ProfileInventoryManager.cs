using Pancake;
using Pancake.Common;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Presenter.Runtime.UI
{
    public class ProfileInventoryManager : GameComponent, ILoadComponent
    {
        public ItemInventoryReference itemInventoryReference;
        public TempHold tempHold;

        [Header("Coin")] public Currency coin;
        public TextMeshProUGUIFormat coinText;
        public TextMeshProUGUIFormat maxCoinText;
        public TextMeshProUGUIFormat coinGoingToBeModifiedText;
        public CanvasGroup coinGoingToBeModifiedCanvasGroup;

        [Header("Gem")] public Currency gem;
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
            tempHold.inventory.OnItemAddedOrIncreased += OnTempItemAddedOrIncreased;
            itemInventoryReference.inventory.OnItemDecreased += OnItemDecreased;
            tempHold.inventory.OnInventoryCleared += OnAllTempItemClear;
            SetAllAlpha(0);
        }

        

        public void OnDisable()
        {
            itemInventoryReference.inventory.OnItemAddedOrIncreased -= OnItemAddedOrIncreased;
            tempHold.inventory.OnItemAddedOrIncreased -= OnTempItemAddedOrIncreased;
            itemInventoryReference.inventory.OnItemDecreased -= OnItemDecreased;
            tempHold.inventory.OnInventoryCleared += OnAllTempItemClear;
        }

        private void Start()
        {
            if (itemInventoryReference.inventory.TryGetItem(coin, out var coinCount)) coinText.SetTextInt(coinCount);
            if (itemInventoryReference.inventory.TryGetItem(gem, out var gemCount)) gemText.SetTextInt(gemCount);
            workerText.SetTextInt(worker);
            weightText.SetTextFloat(weight.x);
            maxWeightText.SetTextFloat(weight.y);
            levelText.SetTextInt(level);
        }

        private void OnItemAddedOrIncreased(Item item, int amount, int count, bool isAdded)
        {
            if (item == coin)
            {
                coinText.SetTextInt(count);
                maxCoinText.SetTextInt(amount);
            }
            else if (item == gem)
            {
                gemText.SetTextInt(count);
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
                coinText.SetTextInt(count);
            }
            else if (item == gem)
            {
                gemText.SetTextInt(count);
            }
            else if (item is IWeight weightedItem)
            {
                weight.x -= weightedItem.Weight * amount;
                weightText.SetTextFloat(weight.x);
            }
        }

        private void OnTempItemAddedOrIncreased(Item item, int amount, int count, bool isAdded)
        {
            if (item == coin)
            {
                coinGoingToBeModifiedText.SetTextInt(-amount);
                coinGoingToBeModifiedCanvasGroup.alpha = 1;
            }
            else if (item == gem)
            {
                gemGoingToBeModifiedText.SetTextInt(-amount);
                gemGoingToBeModifiedCanvasGroup.alpha = 1;
            }
            else if (item is IWeight weightedItem)
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