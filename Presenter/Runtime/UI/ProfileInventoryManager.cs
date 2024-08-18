using Pancake;
using Pancake.Common;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
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

        [Header("Gem")]
        public TMPFormat gemText;
        public TMPFormat gemGoingToBeModifiedText;
        public CanvasGroup gemGoingToBeModifiedCanvasGroup;

        [Header("Worker")] public int worker;
        public TMPFormat workerText;
        public TMPFormat maxWorkerText;
        public TMPFormat workerGoingToBeModifiedText;
        public CanvasGroup workerGoingToBeModifiedCanvasGroup;

        [Header("Weight")] [BarAttribute.Bar] public Vector2 weight;
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
            playerInventoryReference.inventory.OnAddedOrIncreased += OnAddedOrIncreased;
            playerInventoryReference.inventoryPreview.OnAddedOrIncreased += OnTempAddedOrIncreased;
            playerInventoryReference.inventory.OnDecreased += OnDecreased;
            playerInventoryReference.inventoryPreview.OnInventoryCleared += OnAllTempItemClear;
            SetAllAlpha(0);
        }

        

        public void OnDisable()
        {
            playerInventoryReference.inventory.OnAddedOrIncreased -= OnAddedOrIncreased;
            playerInventoryReference.inventoryPreview.OnAddedOrIncreased -= OnTempAddedOrIncreased;
            playerInventoryReference.inventory.OnDecreased -= OnDecreased;
            playerInventoryReference.inventoryPreview.OnInventoryCleared += OnAllTempItemClear;
        }

        private void Start()
        {
            coinText.SetTextInt(playerInventoryReference.coins);
            gemText.SetTextInt(playerInventoryReference.gems);
            workerText.SetTextInt(worker);
            weightText.SetTextFloat(weight.x);
            maxWeightText.SetTextFloat(weight.y);
            levelText.SetTextInt(level);
        }

        private void OnAddedOrIncreased(Item item, int amount, int count, bool isAdded)
        {
            if (item is IWeight weightedItem)
            {
                weight.x += weightedItem.Weight * amount;
                weightText.SetTextFloat(weight.x);
                maxWeightText.SetTextFloat(weight.y);
            }
        }

        private void OnDecreased(Item item, int amount, int count)
        {
            if (item is IWeight weightedItem)
            {
                weight.x -= weightedItem.Weight * amount;
                weightText.SetTextFloat(weight.x);
            }
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