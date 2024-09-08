using System;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.DragAndDrops;
using Soul.Model.Runtime.Items;
using UnityEngine;
using UnityEngine.UI;
using UnityProgressBar;


namespace Soul.Presenter.Runtime.DragAndDrops
{
    public class DragAndDropItem : DragAndDropSelectableController<Item>
    {
        [SerializeField] private Image icon;
        [SerializeField] private ProgressBar inventoryProgressBar;
        [SerializeField] private TMPFormat currentMaxText;

        private void Awake()
        {
            currentMaxText.StoreFormat();
        }

        public void Setup(Camera mainCamera, Item selectedData, float current, float max)
        {
            base.Setup(mainCamera, selectedData);
            icon.sprite = data.Icon;
            UpdateCount(current, max);
        }

        public void UpdateCount(float current, float max)
        {
            inventoryProgressBar.Value = current / max;
            currentMaxText.SetTextFloat(current, max);
        }

        protected override void CanDrop()
        {
        }

        protected override void DropBusy()
        {
        }

        protected override void OnSuccessfulDrop()
        {
        }

        protected override void OnDropFailed()
        {
        }

        protected override void NoDropAbleFound()
        {
        }
    }
}