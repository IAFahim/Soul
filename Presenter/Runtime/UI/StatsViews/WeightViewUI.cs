using System;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.LookUpTables;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Inventories;
using Soul.Model.Runtime.Items;
using TMPro;
using UnityEngine;
using Soul.Model.Runtime.Utils;

namespace Soul.Presenter.Runtime.UI.StatsViews
{
    [Serializable]
    public class WeightViewUI : PreviewStats
    {
        [SerializeField] protected TMPFormat weightText;
        [SerializeField] protected TMPFormat weightMaxText;

        private PlayerFarmReference _playerFarmReference;
        private PriceLookUpTable _priceLookUpTable;

        public void Setup(PlayerFarmReference playerFarmReference, PriceLookUpTable priceLookUpTable)
        {
            _playerFarmReference = playerFarmReference;
            _priceLookUpTable = priceLookUpTable;
            _playerFarmReference.inventory.OnItemChanged += InventoryOnItemChanged;
            SetupToggle();
            SetupPreview(_playerFarmReference.weightPreview);
            CalculateTotalWeightInInventoryAndShow();
        }

        public void CalculateTotalWeightInInventoryAndShow()
        {
            int weight = 0;
            foreach (var item in _playerFarmReference.inventory.GetAll())
            {
                if (item.Key is IWeight weightedItem)
                {
                    weight += weightedItem.Weight * item.Value;
                }
            }

            int maxWeight = _playerFarmReference.weight.Value.Max;
            _playerFarmReference.weight.Value.SetWithoutNotify(weight, maxWeight);
            SetWeightCurrentMax();
        }

        private void InventoryOnItemChanged(InventoryChangeEventArgs<Item, int> changeEventArgs)
        {
            if (changeEventArgs.ChangeType == InventoryChangeType.Added ||
                changeEventArgs.ChangeType == InventoryChangeType.Increased)
            {
                OnAddedOrIncreased(changeEventArgs.Key, changeEventArgs.NewAmount, changeEventArgs.ChangeAmount);
            }
            else if (changeEventArgs.ChangeType == InventoryChangeType.Decreased)
            {
                OnDecreased(changeEventArgs.Key, changeEventArgs.NewAmount, changeEventArgs.ChangeAmount);
            }
        }

        private void OnAddedOrIncreased(Item item, int newAmount, int changeAmount)
        {
            if (item is IWeight weightedItem)
            {
                int itemWeight = weightedItem.Weight * changeAmount;
                var extraWeight = Mathf.Clamp(
                    _playerFarmReference.weight.Value.Current + itemWeight - _playerFarmReference.weight.Value.Max,
                    0, int.MaxValue
                );
                if (extraWeight > 0)
                {
                    _priceLookUpTable.TryGetValue(item, out var price);
                    _playerFarmReference.coins.Value += extraWeight * price;
                    _playerFarmReference.inventory.TryDecrease(item, extraWeight);
                }
                else
                {
                    _playerFarmReference.weight.Value.Current += itemWeight;
                    SetWeightCurrentMax();
                }
            }
        }

        private void SetWeightCurrentMax()
        {
            weightText.SetTextInt(_playerFarmReference.weight.Value.Current);
            weightMaxText.SetTextInt(_playerFarmReference.weight.Value.Max);
        }

        public override void Dispose()
        {
            base.Dispose();
            _playerFarmReference.inventory.OnItemChanged -= InventoryOnItemChanged;
        }

        private void OnDecreased(Item item, int newAmount, int changeAmount)
        {
            if (item is IWeight weightedItem)
            {
                int itemWeight = weightedItem.Weight * changeAmount;
                _playerFarmReference.weight.Value.Current -= itemWeight;
                weightText.SetTextInt(_playerFarmReference.weight.Value.Current);
            }
        }

        public override GameObject LoadComponents(GameObject gameObject, string title)
        {
            var container = base.LoadComponents(gameObject, title);
            weightText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("current");
            weightMaxText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("max");
            return container;
        }
    }
}