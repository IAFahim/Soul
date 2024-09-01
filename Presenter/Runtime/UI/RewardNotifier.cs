using Alchemy.Inspector;
using Pancake;
using Pancake.Common;
using Pancake.Pools;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Limits;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityProgressBar;

namespace Soul.Presenter.Runtime.UI
{
    public class RewardNotifier : GameComponent, ILoadComponent
    {
        [FormerlySerializedAs("countLimitText")] [SerializeField] TMPFormat addedText;
        [SerializeField] TMPFormat totalText;
        [SerializeField] Image icon;
        [SerializeField] ProgressBar progressBar;
        [SerializeField, DisableInEditMode] Item itemReference;

        private IRemoveCallBack<Item> _removeCallBack;

        private void Awake()
        {
            addedText.StoreFormat();
            totalText.StoreFormat();
        }

        public void Setup(Item item, int newAmount, int added, LimitIntStruct limitInt,
            IRemoveCallBack<Item> removeCallBack)
        {
            totalText.TMP.SetText(totalText, newAmount);
            addedText.TMP.SetText("+" + addedText, added);
            icon.sprite = item.Icon;
            progressBar.Value = limitInt;
            itemReference = item;
            _removeCallBack = removeCallBack;
            App.Delay(3f, OnComplete);
        }

        private void OnComplete()
        {
            GameObject.Return();
            _removeCallBack.RemoveSelf(itemReference);
        }

        void ILoadComponent.OnLoadComponents()
        {
            addedText.TMP = GetComponentInChildren<TMP_Text>();
            totalText.TMP = GetComponentsInChildren<TMP_Text>()[^1];
            icon = GetComponents<Image>()[^1];
            addedText.StoreFormat();
            totalText.StoreFormat();
        }
    }
}