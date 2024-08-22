using Alchemy.Inspector;
using Pancake;
using Pancake.Common;
using Pancake.Pools;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Limits;
using UnityEngine;
using UnityEngine.UI;
using UnityProgressBar;

namespace Soul.Presenter.Runtime.UI
{
    public class RewardNotifier : GameComponent, ILoadComponent
    {
        [SerializeField] TMPFormat countLimitText;
        [SerializeField] Image icon;
        [SerializeField] ProgressBar progressBar;
        [SerializeField, DisableInEditMode] Item itemReference;

        private IRemoveReference<Item> _removeReference;

        private void Awake()
        {
            countLimitText.StoreFormat();
        }

        public void Setup(int count, LimitStruct limit, Item item, IRemoveReference<Item> removeReference)
        {
            countLimitText.TMP.SetText("+" + countLimitText, count);
            icon.sprite = item.Icon;
            progressBar.Value = count / (float)limit.Max;
            itemReference = item;
            _removeReference = removeReference;
            App.Delay(3f, OnComplete);
        }

        private void OnComplete()
        {
            _removeReference.RemoveSelf(itemReference);
            GameObject.Return();
        }

        void ILoadComponent.OnLoadComponents()
        {
            countLimitText = GetComponentInChildren<TMPFormat>();
            icon = GetComponents<Image>()[^1];
        }
    }
}