using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.DragAndDrops;
using Soul.Model.Runtime.Items;
using UnityEngine;
using UnityEngine.UI;
using UnityProgressBar;
using LitMotion;
using LitMotion.Extensions;

namespace Soul.Presenter.Runtime.DragAndDrops
{
    public class DragAndDropItem : DragAndDropSelectableController<Item>
    {
        [SerializeField] private Image icon;
        [SerializeField] private Color iconColor;
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
            iconColor = icon.color;
        }

        public void UpdateCount(float current, float max)
        {
            inventoryProgressBar.Value = current / max;
            currentMaxText.SetTextFloat(current, max);
        }

        protected override void CanDrop()
        {
            StopAllTween();
            Debug.Log("Can Drop");
        }

        protected override void DropBusy()
        {
            // Stop other motions if they are active
            StopAllTween();
            Debug.Log("Drop Busy");
        }

        private void StopAllTween()
        {
            
        }

        protected override void OnSuccessfulDrop()
        {
            StopAllTween();
            Debug.Log("Successful Drop");
        }

        protected override void OnDropFailed()
        {
            StopAllTween();
            Debug.Log("Drop Failed");
        }

        protected override void NoDropAbleFound()
        {
            StopAllTween();
            icon.color = iconColor;
        }
    }
}