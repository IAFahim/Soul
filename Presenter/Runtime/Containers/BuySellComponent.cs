using Pancake.UI;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.LookUpTables;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.PoolAbles;
using UnityEngine;
using UnityEngine.UI;
using UnityProgressBar;

namespace Soul.Presenter.Runtime.Containers
{
    public class BuySellComponent : PoolAbleComponent
    {
        public UIButton button;
        public Image backgroundImage;
        public Image iconImage;
        public TMPFormat priceTMPFormat;
        public ProgressBar progressBar;

        private PlayerFarmReference _playerFarmReference;
        private Item _item;
        private int _max;
        private int _current;
        private int _price;

        public void Setup(bool isBuy, PlayerFarmReference playerFarmReference, PriceLookUpTable priceLookUpTable,
            Item item,
            Sprite background)
        {
            _playerFarmReference = playerFarmReference;
            _item = item;

            button.onHoldEvent = null;
            button.onHoldEvent += (isBuy ? BuySingle : SellSingle);

            priceLookUpTable.TryGetValue(item, out _price);
            SetUI(item, _price, background);

            _playerFarmReference.inventory.TryGetValue(item, out _current);
            _playerFarmReference.inventory.TryGetMaxValue(item, out _max);
            Set(item, _current);
        }

        private void OnHoldEvent(float obj)
        {
            throw new System.NotImplementedException();
        }

        private void SetUI(Item item, int price, Sprite background)
        {
            priceTMPFormat.SetTextFloat(price);
            iconImage.sprite = item;
            backgroundImage.sprite = background;
        }

        public void Set(Item item, int current)
        {
            _current = current;
            progressBar.SetValueWithoutNotify(_current / (float)_max);
        }


        private void BuySingle(float _)
        {
            if (_current >= _max) return;
            if (_playerFarmReference.coins.Value < _price) return;
            _playerFarmReference.coins.Value -= _price;
            _playerFarmReference.inventory.AddOrIncrease(_item, 1);
        }

        private void SellSingle(float _)
        {
            if (_current <= 0) return;
            _playerFarmReference.coins.Value += _price;
            _playerFarmReference.inventory.TryDecrease(_item, 1);
        }
    }
}